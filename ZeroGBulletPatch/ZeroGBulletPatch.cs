using BepInEx;
using HarmonyLib;
using System;
using UnityEngine;

namespace ZeroGBulletPatch
{
    
    [BepInPlugin(ModId, ModName, "1.0.0")]
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
    class GeneralInputZeroGPatch
    {
        // remove speed compensation (makes zero-g bullets actually shoot straight)
        private static void Postfix(GeneralInput __instance)
        {
            if (((CharacterData)Traverse.Create(__instance).Field("data").GetValue()).weaponHandler.gun.gravity == 0)
            {
                __instance.aimDirection -= Vector3.up * 0.13f / Mathf.Clamp(((CharacterData)Traverse.Create(__instance).Field("data").GetValue()).weaponHandler.gun.projectileSpeed, 1f, 100f);
            }
        }
    }
}
