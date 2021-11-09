using BepInEx;
using HarmonyLib;
using System;
using UnityEngine;
using ZeroGBulletPatch.Extensions;

namespace ZeroGBulletPatch
{
    
    [BepInPlugin(ModId, ModName, "1.1.0")]
    [BepInProcess("Rounds.exe")]
    public class ZeroGBulletPatch : BaseUnityPlugin
    {
        private const string ModId = "com.dk.rounds.plugins.zerogpatch";
        private const string ModName = "ZeroGBulletPatch";

        private void Awake()
        {
            var harmony = new Harmony(ModId);
            harmony.PatchAll();
        }
        private void Start()
        {
        }
    }

    [Serializable]
    [HarmonyPatch(typeof(GeneralInput), "Update")]
    class GeneralInputArcTrajectoryCompensationPatch
    {
        // remove speed compensation (makes zero-g bullets actually shoot straight)
        private static void Postfix(GeneralInput __instance)
        {
            var gun = ((CharacterData)Traverse.Create(__instance).Field("data").GetValue()).weaponHandler.gun;
            if (gun.gravity == 0 || gun.GetAdditionalData().arcTrajectoryRotationalCompensationDisabled)
            {
                __instance.aimDirection -= Vector3.up * 0.13f / Mathf.Clamp(((CharacterData)Traverse.Create(__instance).Field("data").GetValue()).weaponHandler.gun.projectileSpeed, 1f, 100f);
            }
        }
    }
}
