using HarmonyLib;

namespace StupidTemplate.Patches.Internal
{
    [HarmonyPatch(typeof(VRRig), "OnDisable")]
    public class RigPatch
    {
        public static bool Prefix(VRRig __instance) =>
            __instance != VRRig.LocalRig;
    }

    [HarmonyPatch(typeof(VRRig), "PostTick")]
    public class RigPatch2
    {
        public static bool Prefix(VRRig __instance) =>
            !__instance.isLocal || __instance.enabled;
    }
}
