using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ProfileReady : NetworkBehaviour
{
    private readonly int _readyHash = Animator.StringToHash("isReady");

    [SerializeField] private Animator _hostAnimator;
    [SerializeField] private Animator _clientAnimator;

    [ServerRpc(RequireOwnership = false)]
    public void SetReadyServerRpc(GameRole role, bool state)
    {
        SetReadyClientRpc(role, state);
    }

    [ClientRpc]
    private void SetReadyClientRpc(GameRole role, bool state)
    {
        if (role == GameRole.Host)
        {
            _hostAnimator.SetBool(_readyHash, state);
        }
        else
        {
            _clientAnimator.SetBool(_readyHash, state);
        }
    }
}
