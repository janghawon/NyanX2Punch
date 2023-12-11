using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerAnimation : NetworkBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private PlayerState _playerState;

    private readonly int _isMoveHash = Animator.StringToHash("isMove");
    private readonly int _isAtkHash = Animator.StringToHash("isAtk");
    private readonly int _isJumpHash = Animator.StringToHash("isJump");

    private NetworkVariable<bool> _flipValue;
    private NetworkVariable<float> _moveValue;
    private NetworkVariable<bool> _atkValue;
    private NetworkVariable<bool> _jumpValue;

    private void Awake()
    {
        _flipValue = new NetworkVariable<bool>();
        _moveValue = new NetworkVariable<float>();
        _atkValue = new NetworkVariable<bool>();
        _jumpValue = new NetworkVariable<bool>();
    }

    public override void OnNetworkSpawn()
    {
        _flipValue.OnValueChanged += HandleFlipValueChange;
        _moveValue.OnValueChanged += HandleMoveValueChange;
        _atkValue.OnValueChanged += HandleAtkValueChange;
        _jumpValue.OnValueChanged += HandleJumpValueChange;
    }

    public override void OnNetworkDespawn()
    {
        _flipValue.OnValueChanged -= HandleFlipValueChange;
        _moveValue.OnValueChanged -= HandleMoveValueChange;
        _atkValue.OnValueChanged -= HandleAtkValueChange;
        _jumpValue.OnValueChanged -= HandleJumpValueChange;
    }

    #region AttackLogic
    public void SetAtk(bool value)
    {
        if(_atkValue.Value != value)
        {
            SetAtkValueServerRpc(value);
        }
        _animator.SetBool(_isAtkHash, value);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SetAtkValueServerRpc(bool value)
    {
        _atkValue.Value = value;
    }

    private void HandleAtkValueChange(bool previousValue, bool newValue)
    {
        _animator.SetBool(_isAtkHash, newValue);
    }
    #endregion

    #region FlipLogic
    public void FlipController()
    {
        if (_moveValue.Value == 0 || _playerState.IsOnAttack) return;

        if(_spriteRenderer.flipX != _moveValue.Value > 0)
        {
            _spriteRenderer.flipX = _moveValue.Value > 0;
            SetValueFlipServerRpc(_spriteRenderer.flipX);
        }
    }

    [ServerRpc]
    private void SetValueFlipServerRpc(bool value)
    {
        _flipValue.Value = value;
    }

    private void HandleFlipValueChange(bool previousValue, bool newValue)
    {
        _spriteRenderer.flipX = newValue;
    }
    #endregion

    #region MoveLogic
    public void SetMove(float Xdir)
    {
        if (_playerState.IsOnJump)
        {
            _animator.SetBool(_isMoveHash, false);
            return;
        }

        if (_moveValue.Value != Xdir)
        {
            SetValueMoveServerRpc(Xdir);
        }
        
        _animator.SetBool(_isMoveHash, Xdir != 0);
    }

    [ServerRpc]
    private void SetValueMoveServerRpc(float Xdir)
    {
        if (_playerState.IsOnJump) return;
        _moveValue.Value = Xdir;
    }

    private void HandleMoveValueChange(float previousValue, float newValue)
    {
        _animator.SetBool(_isMoveHash, newValue != 0);
    }
    #endregion

    #region JumpLogic
    public void SetJump(bool value)
    {
        if(_jumpValue.Value != value)
        {
            SetJumpValueServerRpc(value);
        }
        _animator.SetBool(_isJumpHash, value);
    }

    [ServerRpc]
    private void SetJumpValueServerRpc(bool value)
    {
        _jumpValue.Value = value;
    }

    private void HandleJumpValueChange(bool previousValue, bool newValue)
    {
        _animator.SetBool(_isJumpHash, newValue);
    }
    #endregion
}
