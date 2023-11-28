using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerAnimation : MonoBehaviour
{
    [SerializeField] private Animator _animator;
    [SerializeField] private SpriteRenderer _spriteRenderer;

    private readonly int _isMoveHash = Animator.StringToHash("Speed");
    private readonly int _isAtkHash = Animator.StringToHash("isAtk");
    private readonly int _atkCountHash = Animator.StringToHash("AtkCount");

    private bool _isInDodge;

    public void SetMove(float value)
    {
        _animator.SetFloat(_isMoveHash, value);
    }
    public void SetAttack(bool atkState, int atkCount)
    {
        if (_isInDodge) return;

        _animator.SetBool(_isAtkHash, atkState);
        _animator.SetInteger(_atkCountHash, atkCount);
    }
    public void FlipController(float xDir)
    {
        bool isRightTurn = xDir > 0 && _spriteRenderer.flipX;
        bool isLeftTurn = xDir < 0 && !_spriteRenderer.flipX;

        if (isRightTurn || isLeftTurn)
        {
            Flip();
        }
    }
    public void Flip()
    {
        _spriteRenderer.flipX = !_spriteRenderer.flipX;
    }
}
