using BepInEx;
using System.ComponentModel;

namespace StupidTemplate.Patches
{
    [Description(StupidTemplate.PluginInfo.Description)]
    [BepInPlugin(StupidTemplate.PluginInfo.GUID, StupidTemplate.PluginInfo.Name, StupidTemplate.PluginInfo.Version)]
    public class HarmonyPatches : BaseUnityPlugin
    {
        private void OnEnable()
        {
            Menu.ApplyHarmonyPatches();
        }

        private void OnDisable()
        {
            Menu.RemoveHarmonyPatches();
        }
    }
}
