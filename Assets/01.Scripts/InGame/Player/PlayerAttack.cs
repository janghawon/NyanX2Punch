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
    [SerializeField] private GameObject _sparkImpact;
    [SerializeField] private InputReader _inputReader;
    [SerializeField] private PlayerAnimation _playerAnimation;
    [SerializeField] private PlayerState _playerState;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    private bool _isLookRight => !_spriteRenderer.flipX;

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
        if (_playerState.IsOnJump || _playerState.IsOnAttack) return;

        _playerAnimation.SetAtk(true);
    }
    
    public void AttackEndEvent()
    {
        _playerAnimation.SetAtk(false);
    }

    public void AttackLogic()
    {
        if (!IsOwner) return;

        Collider2D[] colliders = Physics2D.OverlapCircleAll(transform.position, _detectRange);
        foreach (Collider2D collider in colliders)
        {
            if (collider == _myCol) continue;

            if (collider.TryGetComponent<PlayerHealth>(out PlayerHealth ph))
            {
                Vector3 dir = (ph.hitTrm.position - transform.position).normalized;
                if ((dir.x >= 0 && !_isLookRight) || (dir.x < 0 && _isLookRight)) return;

                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

                MakeFeedbackServerRpc(dir, ph.hitTrm.position, angle);
                _gameBar.OnChangeGameBarValueServerRpc(_atkValue, OwnerClientId);
                ph.Hit(dir);
            }
        }
    }

    [ServerRpc]
    private void MakeFeedbackServerRpc(Vector3 dir, Vector3 pos, float angle)
    {
        MakeFeedbackClientRpc(pos, angle);
    }

    [ClientRpc]
    private void MakeFeedbackClientRpc(Vector3 pos, float angle)
    {
        FeedbackManager.Instance.MakeFxServerRpc(FXType.impact, pos, Quaternion.Euler(0, 0, angle));
        FeedbackManager.Instance.MakeFxServerRpc(FXType.spark, pos);

        FeedbackManager.Instance.ShaekScreen(new Vector3(0.1f, 0.1f, 0));
        FeedbackManager.Instance.StopTime(0.02f);
    }
}
