using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerDodge : NetworkBehaviour
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private PlayerAnimation _playerAnimation;
    [SerializeField] private Rigidbody2D _rigid;
    [SerializeField] private PlayerMovement _movement;
    [SerializeField] private TrailRenderer _trail;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;
        _inputReader.DodgeEvent += HandleDodge;
    }

    public override void OnNetworkDespawn()
    {
        if (!IsOwner) return;
        _inputReader.DodgeEvent -= HandleDodge;
    }

    private void HandleDodge()
    {
        if (PlayerState.IsOnJump) return;

        PlayerState.IsOnDodge = true;
        _playerAnimation.SetDodge(true);
    }

    public void DodgeStartEvent()
    {
        _playerAnimation.SetDodge(false);
        _trail.enabled = true;
        _movement.movementSpeed = 0.35f;
    }

    public void DodgeSpeedNormalize()
    {
        _movement.movementSpeed = 0.15f;
        _trail.enabled = false;
    }
}
