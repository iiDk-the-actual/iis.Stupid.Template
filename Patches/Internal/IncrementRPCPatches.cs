using HarmonyLib;
using Photon.Pun;

namespace StupidTemplate.Patches.Internal
{
    public class IncrementRPCPatches
    {
        [HarmonyPatch(typeof(VRRig), "IncrementRPC", typeof(PhotonMessageInfoWrapped), typeof(string))]
        public class NoIncrementRPC
        {
            private static bool Prefix(PhotonMessageInfoWrapped info, string sourceCall) =>
                false;
        }

        [HarmonyPatch(typeof(GorillaNot), "IncrementRPCCall", typeof(PhotonMessageInfo), typeof(string))]
        public class NoIncrementRPCCall
        {
            private static bool Prefix(PhotonMessageInfo info, string callingMethod = "") =>
                false;
        }

        [HarmonyPatch(typeof(GorillaNot), "IncrementRPCCallLocal")]
        public class NoIncrementRPCCallLocal
        {
            private static bool Prefix(PhotonMessageInfoWrapped infoWrapped, string rpcFunction) =>
                false;
        }
    }
}
