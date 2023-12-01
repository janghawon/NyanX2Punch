using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerAttack : NetworkBehaviour
{
    private GameBar _gameBar;
    [SerializeField] private Collider2D _myCol;
    [SerializeField] private int _atkValue = 5;
    [SerializeField] private float _detectRange = 5.0f;
    [SerializeField] private GameObject _hitImpact;
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private PlayerAnimation _playerAnimation;

    private void Awake()
    {
        _gameBar = GameObject.Find("Canvas/EmptyBar").GetComponent<GameBar>();
    }

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
        if (PlayerState.IsOnJump) return;

        _playerAnimation.SetAttack(true);
    }

    public void AttackEndEvent()
    {
        _playerAnimation.SetAttack(false);
    }

    public void AttackLogic()
    {
        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _detectRange);
        foreach (Collider2D collider in colliders)
        {
            if (collider == _myCol) continue;
            if (collider.TryGetComponent<PlayerHealth>(out PlayerHealth ph))
            {
                Vector3 dir = (collider.transform.position - transform.position).normalized;
                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
                Instantiate(_hitImpact, ph.hitTrm.position, Quaternion.Euler(0, 0, angle));
                _gameBar.OnChangeGameBarValue(_atkValue);
                FeedbackManager.Instance.ShaekScreen();
                FeedbackManager.Instance.StopTime(0.02f);
            }
        }
    }
}
