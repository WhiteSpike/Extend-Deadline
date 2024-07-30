using ExtendDeadline;
using HarmonyLib;
using Unity.Netcode;
using UnityEngine;

namespace ExtendDeadline.Patches.RoundComponents
{
    [HarmonyPatch(typeof(StartOfRound))]
    internal static class StartOfRoundPatcher
    {
        [HarmonyPrefix]
        [HarmonyPatch(nameof(StartOfRound.Awake))]
        static void InitLguStore(StartOfRound __instance)
        {
            Plugin.mls.LogDebug("Initiating components...");
            if (__instance.NetworkManager.IsHost || __instance.NetworkManager.IsServer)
            {
                GameObject behaviour = Object.Instantiate(Plugin.networkPrefab);
                behaviour.hideFlags = HideFlags.HideAndDontSave;
                behaviour.GetComponent<NetworkObject>().Spawn();
                Plugin.mls.LogDebug("Spawned behaviour...");
            }
        }
    }
}
