using BepInEx;
using BepInEx.Logging;
using HarmonyLib;
using UnityEngine;

namespace RepoSpinTeleportMod;

[BepInPlugin(PluginInfo.GUID, PluginInfo.NAME, PluginInfo.VERSION)]
public class RepoSpinTeleportMod : BaseUnityPlugin
{
    internal static RepoSpinTeleportMod Instance { get; private set; } = null!;
    internal new static ManualLogSource Logger => Instance._logger;
    private ManualLogSource _logger => base.Logger;
    internal Harmony? Harmony { get; set; }

    private void Awake()
    {
        Instance = this;

        gameObject.transform.parent = null;
        gameObject.hideFlags = HideFlags.HideAndDontSave;
        gameObject.AddComponent<SpinTeleportModBehaviour>();

        Patch();

        Logger.LogInfo($"{Info.Metadata.GUID} v{Info.Metadata.Version} loaded.");
    }

    internal void Patch()
    {
        Harmony ??= new Harmony(Info.Metadata.GUID);
        Harmony.PatchAll();
    }

    internal void Unpatch()
    {
        Harmony?.UnpatchSelf();
    }
}

internal static class PluginInfo
{
    public const string GUID = "RepoMod.RepoSpinTeleportMod";
    public const string NAME = "Spin Teleport Mod";
    public const string VERSION = "1.1.0";
}
