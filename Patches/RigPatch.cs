using HarmonyLib;
using UnityEngine;

namespace StupidTemplate.Patches
{
    [HarmonyPatch(typeof(VRRig), "OnDisable")]
    public class GhostPatch : MonoBehaviour
    {
        public static bool Prefix(VRRig __instance) =>
            __instance != VRRig.LocalRig;
    }
}
