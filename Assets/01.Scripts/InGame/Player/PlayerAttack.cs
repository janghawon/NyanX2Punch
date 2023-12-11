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
    [SerializeField] private float _atkCool;
    private float _currentTime;
    private bool _canAtk = true;

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
        if (_playerState.IsOnJump || _playerState.IsOnAttack || !_canAtk) return;
        _canAtk = false;

        AudioManager.Instance.PlaySFX(SFXType.swipe);
        _playerAnimation.SetAtk(true);
    }
    
    public void AttackEndEvent()
    {
        _playerAnimation.SetAtk(false);
    }

    private void FixedUpdate()
    {
        if (_canAtk) return;

        _currentTime += Time.deltaTime;

        if(_currentTime >= _atkCool)
        {
            _currentTime = 0;
            _canAtk = true;
        }
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

                _gameBar.OnChangeGameBarValueServerRpc(_atkValue, OwnerClientId);

                float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;

                MakeFeedbackServerRpc(ph.hitTrm.position, angle);
                ph.Hit(dir);
            }
        }
    }

    [ServerRpc]
    private void MakeFeedbackServerRpc(Vector3 pos, float angle)
    {
        FeedbackManager.Instance.MakeFxClientRpc(FXType.impact, pos, Quaternion.Euler(0, 0, angle));
        FeedbackManager.Instance.MakeFxClientRpc(FXType.spark, pos);
        MakeFeedbackClientRpc();
    }

    [ClientRpc]
    private void MakeFeedbackClientRpc()
    {
        AudioManager.Instance.PlaySFX(SFXType.hit);

        FeedbackManager.Instance.ShaekScreen(new Vector3(0.1f, 0.1f, 0));
        FeedbackManager.Instance.StopTime(0.02f, 0.3f);
    }
}
