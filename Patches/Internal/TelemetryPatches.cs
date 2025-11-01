using HarmonyLib;
using JetBrains.Annotations;
using PlayFab.EventsModels;

namespace StupidTemplate.Patches.Internal
{
    public class TelemetryPatches
    {
        public static bool enabled = true;

        [HarmonyPatch(typeof(GorillaTelemetry), "EnqueueTelemetryEvent")]
        public class TelemetryPatch1
        {
            private static bool Prefix(string eventName, object content, [CanBeNull] string[] customTags = null) =>
                !enabled;
        }

        [HarmonyPatch(typeof(GorillaTelemetry), "EnqueueTelemetryEventPlayFab")]
        public class TelemetryPatch2
        {
            private static bool Prefix(EventContents eventContent) =>
                !enabled;
        }
    }
}
