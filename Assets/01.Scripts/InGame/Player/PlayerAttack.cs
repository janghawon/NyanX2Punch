using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerAttack : NetworkBehaviour
{
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private PlayerAnimation _playerAnimation;

    private int _atkCount;

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
        if (_atkCount >= 2) _atkCount = 0;

        _playerAnimation.SetAttack(true, _atkCount);
        _atkCount++;
    }

    public void AttackEndEvent()
    {
        _playerAnimation.SetAttack(false, _atkCount);
    }
}
