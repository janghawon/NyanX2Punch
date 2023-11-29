using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerAttack : NetworkBehaviour
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private PlayerAnimation _playerAnimation;

    public override void OnNetworkSpawn()
    {
        if (!IsOwner) return;
        _inputReader.AttackEvent += HandleAttack;
    }

    public override void OnNetworkDespawn()
    {
        if (!IsOwner) return;
        _inputReader.AttackEvent -= HandleAttack;
    }

    private void HandleAttack()
    {
        _playerAnimation.SetAttack(true);
    }

    public void AttackStartEvent()
    {
        _playerAnimation.SetAttack(false);
    }
}
