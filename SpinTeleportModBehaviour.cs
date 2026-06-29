using System.Collections;
using UnityEngine;

namespace RepoSpinTeleportMod;

internal sealed class SpinTeleportModBehaviour : MonoBehaviour
{
    private int _targetIndex;
    private Coroutine? _teleportRoutine;

    private void OnEnable()
    {
        _teleportRoutine = StartCoroutine(TeleportLoop());
    }

    private void OnDisable()
    {
        if (_teleportRoutine != null)
        {
            StopCoroutine(_teleportRoutine);
            _teleportRoutine = null;
        }
    }

    private void LateUpdate()
    {
        SpinTeleportLogic.ApplyAvatarSpin(PlayerAvatar.instance);
    }

    private IEnumerator TeleportLoop()
    {
        WaitForSeconds wait = new WaitForSeconds(SpinTeleportLogic.TeleportIntervalSeconds);

        while (true)
        {
            yield return wait;
            SpinTeleportLogic.TeleportToNextPlayer(ref _targetIndex);
        }
    }
}
