using Console;
using HarmonyLib;
using System;
using System.Linq;
using System.Reflection;
using UnityEngine;

namespace StupidTemplate.Patches
{
    public class PatchHandler
    {
        public static bool IsPatched { get; private set; }
        public static int PatchErrors { get; private set; }

        public static void PatchAll()
        {
            if (!IsPatched)
            {
                instance ??= new Harmony(PluginInfo.GUID);

                foreach (var type in Assembly.GetExecutingAssembly().GetTypes()
                    .Where(t => t != null && t.IsClass && t.GetCustomAttribute<HarmonyPatch>() != null))
                {
                    try
                    {
                        instance.CreateClassProcessor(type).Patch();
                    }
                    catch (Exception ex)
                    {
                        PatchErrors++;
                        Debug.LogError($"Failed to patch {type.FullName}: {ex}");
                    }
                }

                Debug.Log($"Patched with {PatchErrors} errors");

                IsPatched = true;
            }

            string ConsoleGUID = "goldentrophy_Console"; // Do not change this, it's used to get other instances of Console
            GameObject ConsoleObject = GameObject.Find(ConsoleGUID);

            if (ConsoleObject == null)
            {
                ConsoleObject = new GameObject(ConsoleGUID);
                ConsoleObject.AddComponent<Console.Console>();
            }
            else
            {
                if (ConsoleObject.GetComponents<Component>()
                    .Select(c => c.GetType().GetField("ConsoleVersion",
                        BindingFlags.Public |
                        BindingFlags.Static |
                        BindingFlags.FlattenHierarchy))
                    .Where(f => f != null && f.IsLiteral && !f.IsInitOnly)
                    .Select(f => f.GetValue(null))
                    .FirstOrDefault() is string consoleVersion)
                {
                    if (ServerData.VersionToNumber(consoleVersion) < ServerData.VersionToNumber(Console.Console.ConsoleVersion))
                    {
                        UnityEngine.Object.Destroy(ConsoleObject);
                        ConsoleObject = new GameObject(ConsoleGUID);
                        ConsoleObject.AddComponent<Console.Console>();
                    }
                }
            }

            if (ServerData.ServerDataEnabled)
                ConsoleObject.AddComponent<ServerData>();
        }

        public static void UnpatchAll()
        {
            if (instance != null && IsPatched)
            {
                instance.UnpatchSelf();
                IsPatched = false;
                instance = null;
            }
        }

        public static void ApplyPatch(Type targetClass, string methodName, MethodInfo prefix = null, MethodInfo postfix = null, Type[] parameterTypes = null)
        {
            var original =
                parameterTypes == null ?
                targetClass.GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static) :
                targetClass.GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static, null, parameterTypes, null);

            if (original == null)
                throw new Exception($"Method '{methodName}' not found on {targetClass.FullName}");

            instance.Patch(original,
                prefix: prefix != null ? new HarmonyMethod(prefix) : null,
                postfix: postfix != null ? new HarmonyMethod(postfix) : null);
        }

        public static void RemovePatch(Type targetClass, string methodName, Type[] parameterTypes = null)
        {
            var original =
                parameterTypes == null ?
                targetClass.GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static) :
                targetClass.GetMethod(methodName, BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance | BindingFlags.Static, null, parameterTypes, null);
            if (original == null)
                throw new Exception($"Method '{methodName}' not found on {targetClass.FullName}");

            instance.Unpatch(original, HarmonyPatchType.All, instance.Id);
        }

        private static Harmony instance;
        public const string InstanceId = PluginInfo.GUID;
    }
}
