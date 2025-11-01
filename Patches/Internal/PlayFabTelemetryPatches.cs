using HarmonyLib;
using PlayFab;
using PlayFab.ClientModels;
using PlayFab.Internal;
using System;
using System.Collections.Generic;
using Random = UnityEngine.Random;

namespace StupidTemplate.Patches.Internal
{
    public class PlayFabTelemetryPatches
    {
        [HarmonyPatch(typeof(PlayFabDeviceUtil), "SendDeviceInfoToPlayFab")]
        public class PlayfabUtil01
        {
            private static bool Prefix() =>
                false;
        }

        [HarmonyPatch(typeof(PlayFabClientInstanceAPI), "ReportDeviceInfo")]
        public class PlayfabUtil02
        {
            private static bool Prefix() =>
                false;
        }

        [HarmonyPatch(typeof(PlayFabClientAPI), "ReportDeviceInfo")]
        public class PlayfabUtil03
        {
            private static bool Prefix() =>
                false;
        }

        [HarmonyPatch(typeof(PlayFabDeviceUtil), "GetAdvertIdFromUnity")]
        public class PlayfabUtil04
        {
            private static bool Prefix() =>
                false;
        }

        [HarmonyPatch(typeof(PlayFabClientAPI), "AttributeInstall")]
        public class PlayfabUtil05
        {
            private static bool Prefix() =>
                false;
        }

        [HarmonyPatch(typeof(PlayFabHttp), "InitializeScreenTimeTracker")]
        public class PlayfabUtil06
        {
            private static bool Prefix() =>
                false;
        }

        [HarmonyPatch(typeof(PlayFabClientAPI), "UpdateUserTitleDisplayName")] // Credits to Shiny for letting me use this
        public class DisplayNamePatch
        {
            public static string RandomString(int length = 4)
            {
                string random = "";
                for (int i = 0; i < length; i++)
                {
                    int rand = Random.Range(0, 36);
                    char c = rand < 26
                        ? (char)('A' + rand)
                        : (char)('0' + (rand - 26));
                    random += c;
                }

                return random;
            }

            public static void Prefix(ref UpdateUserTitleDisplayNameRequest request, Action<UpdateUserTitleDisplayNameResult> resultCallback, Action<PlayFabError> errorCallback, object customData = null, Dictionary<string, string> extraHeaders = null) =>
                request.DisplayName = RandomString(Random.Range(3, 12));
        }
    }
}
