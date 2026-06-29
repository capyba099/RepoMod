using System.Collections.Generic;
using UnityEngine;

namespace RepoSpinTeleportMod;

internal static class SpinTeleportLogic
{
    internal const float SpinSpeedDegreesPerSecond = 1080f;
    internal const float TeleportIntervalSeconds = 1f;
    internal const float TeleportHeightOffset = 0.15f;

    internal static bool ModifyingNetworkRotation { get; set; }

    private static float _spinAngle;

    internal static Quaternion CurrentSpinRotation => Quaternion.Euler(0f, _spinAngle, 0f);

    internal static void AdvanceSpin()
    {
        _spinAngle += SpinSpeedDegreesPerSecond * Time.deltaTime;
        if (_spinAngle >= 360f)
        {
            _spinAngle -= 360f;
        }
    }

    internal static bool IsLocalVisuals(PlayerAvatarVisuals visuals)
    {
        if (visuals == null || visuals.isMenuAvatar)
        {
            return false;
        }

        PlayerAvatar localAvatar = PlayerAvatar.instance;
        return localAvatar != null
            && localAvatar.isLocal
            && localAvatar.playerAvatarVisuals == visuals;
    }

    internal static bool CanTeleport()
    {
        return PlayerAvatar.instance != null && PlayerAvatar.instance.isLocal;
    }

    internal static void ApplySpin(PlayerAvatarVisuals visuals)
    {
        if (!IsLocalVisuals(visuals))
        {
            return;
        }

        AdvanceSpin();

        visuals.ShowSelfOverride(0.25f);

        if (visuals.meshParent != null)
        {
            visuals.meshParent.transform.Rotate(0f, SpinSpeedDegreesPerSecond * Time.deltaTime, 0f, Space.Self);
        }
    }

    internal static void ApplyAvatarSpin(PlayerAvatar avatar)
    {
        if (avatar == null || !avatar.isLocal)
        {
            return;
        }

        if (avatar.playerAvatarVisuals != null)
        {
            ApplySpin(avatar.playerAvatarVisuals);
        }
    }

    internal static Quaternion ApplySpinToRotation(Quaternion rotation)
    {
        return rotation * CurrentSpinRotation;
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
