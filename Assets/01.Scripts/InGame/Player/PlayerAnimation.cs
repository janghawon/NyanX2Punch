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
    private readonly int _jumpVlaueHash = Animator.StringToHash("jumpValue");

    public bool isRight;
    private float _moveValue;

    public void SetMove(float Xdir)
    {
        _moveValue = Xdir;
        
        FlipController();

        if (_playerState.IsOnJump) return;
        _animator.SetBool(_isMoveHash, Xdir != 0);
    }

    public void SetJump(bool JumpState, float jVlaue)
    {
        if (_playerState.IsOnDodge) return;

        _animator.SetBool(_isJumpHash, JumpState);
        _animator.SetFloat(_jumpVlaueHash, jVlaue);
    }

    public void SetAttack(bool atkState)
    {
        _animator.SetBool(_isAtkHash, atkState);
    }

    private void FlipController()
    {
        if (_moveValue == 0 || _playerState.IsOnAttack) return;
        isRight = _moveValue > 0;
        _spriteRenderer.flipX = isRight;
    }
}
