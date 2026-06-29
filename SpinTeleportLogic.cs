using System.Collections.Generic;
using UnityEngine;

namespace RepoSpinTeleportMod;

internal static class SpinTeleportLogic
{
    internal const float SpinSpeedDegreesPerSecond = 1080f;
    internal const float TeleportIntervalSeconds = 1f;
    internal const float TeleportHeightOffset = 0.15f;

    internal static bool IsActive()
    {
        if (PlayerAvatar.instance == null || !PlayerAvatar.instance.isLocal)
        {
            return false;
        }

        if (LevelGenerator.Instance == null || !LevelGenerator.Instance.Generated)
        {
            return false;
        }

        if (SemiFunc.MenuLevel())
        {
            return false;
        }

        if (GameDirector.instance == null || GameDirector.instance.currentState != GameDirector.gameState.Main)
        {
            return false;
        }

        if (PlayerAvatar.instance.isDisabled)
        {
            return false;
        }

        return true;
    }

    internal static void ApplySpin(PlayerAvatarVisuals visuals)
    {
        if (!IsActive() || visuals.playerAvatar != PlayerAvatar.instance)
        {
            return;
        }

        visuals.transform.Rotate(0f, SpinSpeedDegreesPerSecond * Time.deltaTime, 0f, Space.World);
    }

    internal static void TeleportToNextPlayer(ref int targetIndex)
    {
        if (!IsActive())
        {
            return;
        }

        List<PlayerAvatar> players = SemiFunc.PlayerGetList();
        List<PlayerAvatar> targets = new List<PlayerAvatar>();

        foreach (PlayerAvatar player in players)
        {
            if (player == null || player.isDisabled || player == PlayerAvatar.instance)
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
        PlayerController controller = PlayerController.instance;

        controller.transform.position = position;
        controller.rb.velocity = Vector3.zero;
        controller.rb.angularVelocity = Vector3.zero;

        avatar.transform.position = position;
        avatar.clientPosition = position;
        avatar.clientPositionCurrent = position;
        avatar.rb.velocity = Vector3.zero;
        avatar.rb.angularVelocity = Vector3.zero;

        if (avatar.playerAvatarVisuals != null)
        {
            avatar.playerAvatarVisuals.visualPosition = position;
            avatar.playerAvatarVisuals.transform.position = position;
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
