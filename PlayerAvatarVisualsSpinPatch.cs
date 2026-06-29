using HarmonyLib;

namespace RepoSpinTeleportMod;

[HarmonyPatch(typeof(PlayerAvatarVisuals), nameof(PlayerAvatarVisuals.Update))]
internal static class PlayerAvatarVisualsSpinPatch
{
    [HarmonyPostfix]
    private static void Postfix(PlayerAvatarVisuals __instance)
    {
        SpinTeleportLogic.ApplySpin(__instance);
    }
}
