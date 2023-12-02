using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer _spriteRenderer;
    [SerializeField] private PlayerState _playerState;

    private readonly int _isMoveHash = Animator.StringToHash("Speed");
    private readonly int _isAtkHash = Animator.StringToHash("isAtk");
    private readonly int _isJumpHash = Animator.StringToHash("isJump");
    private readonly int _jumpVlaueHash = Animator.StringToHash("jumpValue");
    private readonly int _dodgeHash = Animator.StringToHash("isDodge");

    public bool isRight;
    private float _moveValue;
    private Vector3 _prevValue;

    public void SetMove(Vector3 dir)
    {
        _moveValue = _prevValue.x - dir.x;

        FlipController();
        _animator.SetFloat(_isMoveHash, Mathf.Abs(_moveValue));
        _prevValue = dir;
    }

    public void SetJump(bool JumpState, float jVlaue)
    {
        if (_playerState.IsOnDodge) return;

        _animator.SetBool(_isJumpHash, JumpState);
        _animator.SetFloat(_jumpVlaueHash, jVlaue);
    }

    public void SetDodge(bool dodgeState)
    {
        _animator.SetBool(_dodgeHash, dodgeState);
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
