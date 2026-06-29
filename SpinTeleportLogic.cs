using System.Collections.Generic;
using UnityEngine;

namespace RepoSpinTeleportMod;

internal static class SpinTeleportLogic
{
    internal const float SpinSpeedDegreesPerSecond = 1080f;
    internal const float TeleportIntervalSeconds = 1f;
    internal const float TeleportHeightOffset = 0.15f;

    internal static bool CanSpin(PlayerAvatarVisuals visuals)
    {
        if (visuals == null || visuals.isMenuAvatar)
        {
            return false;
        }

        PlayerAvatar localAvatar = PlayerAvatar.instance;
        if (localAvatar == null || !localAvatar.isLocal)
        {
            return false;
        }

        return visuals.playerAvatar == localAvatar;
    }

    internal static bool CanTeleport()
    {
        return PlayerAvatar.instance != null && PlayerAvatar.instance.isLocal;
    }

    internal static void ApplySpin(PlayerAvatarVisuals visuals)
    {
        if (!CanSpin(visuals))
        {
            return;
        }

        visuals.transform.Rotate(0f, SpinSpeedDegreesPerSecond * Time.deltaTime, 0f, Space.World);
    }

    internal static void TeleportToNextPlayer(ref int targetIndex)
    {
        if (!CanTeleport())
        {
            return;
        }

        List<PlayerAvatar> players = SemiFunc.PlayerGetList();
        if (players == null || players.Count == 0)
        {
            return;
        }

        List<PlayerAvatar> targets = new List<PlayerAvatar>();

        foreach (PlayerAvatar player in players)
        {
            if (player == null || player == PlayerAvatar.instance)
            {
                continue;
            }

            targets.Add(player);
        }

        if (targets.Count == 0)
        {
            return;
        }

        if (targetIndex >= targets.Count)
        {
            targetIndex = 0;
        }

        PlayerAvatar target = targets[targetIndex];
        targetIndex = (targetIndex + 1) % targets.Count;

        Vector3 destination = target.transform.position + Vector3.up * TeleportHeightOffset;
        TeleportLocalPlayer(destination);
    }

    private static void TeleportLocalPlayer(Vector3 position)
    {
        PlayerAvatar avatar = PlayerAvatar.instance;

        avatar.transform.position = position;
        avatar.clientPosition = position;
        avatar.clientPositionCurrent = position;

        if (avatar.rb != null)
        {
            avatar.rb.velocity = Vector3.zero;
            avatar.rb.angularVelocity = Vector3.zero;
            avatar.rb.MovePosition(position);
        }

        if (avatar.playerAvatarVisuals != null)
        {
            avatar.playerAvatarVisuals.visualPosition = position;
            avatar.playerAvatarVisuals.transform.position = position;
        }

        if (PlayerController.instance != null)
        {
            PlayerController controller = PlayerController.instance;
            controller.transform.position = position;

            if (controller.rb != null)
            {
                controller.rb.velocity = Vector3.zero;
                controller.rb.angularVelocity = Vector3.zero;
                controller.rb.MovePosition(position);
            }
        }

        if (avatar.localCamera != null)
        {
            avatar.localCamera.Teleported();
        }

        if (CameraPosition.instance != null)
        {
            CameraPosition.instance.transform.position = position;
        }
    }
}
