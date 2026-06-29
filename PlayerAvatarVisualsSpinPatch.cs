using HarmonyLib;
using Photon.Pun;
using UnityEngine;

namespace RepoSpinTeleportMod;

[HarmonyPatch(typeof(PlayerAvatar), nameof(PlayerAvatar.OnPhotonSerializeView))]
internal static class PlayerAvatarNetworkSpinPatch
{
    [HarmonyPrefix]
    private static void Prefix(PlayerAvatar __instance, PhotonStream stream)
    {
        SpinTeleportLogic.ModifyingNetworkRotation =
            stream.IsWriting
            && __instance.isLocal
            && __instance == PlayerAvatar.instance;
    }

    [HarmonyFinalizer]
    private static void Finalizer()
    {
        SpinTeleportLogic.ModifyingNetworkRotation = false;
    }
}

[HarmonyPatch(typeof(PhotonStream), nameof(PhotonStream.SendNext), typeof(Quaternion))]
internal static class PhotonStreamQuaternionSpinPatch
{
    [HarmonyPrefix]
    private static void Prefix(ref Quaternion value)
    {
        if (!SpinTeleportLogic.ModifyingNetworkRotation)
        {
            return;
        }

        value = SpinTeleportLogic.ApplySpinToRotation(value);
    }
}
