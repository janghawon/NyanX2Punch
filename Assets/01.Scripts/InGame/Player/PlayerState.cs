using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public bool IsOnJump;
    public bool IsOnDodge;
    public bool IsOnAttack;
    public bool IsOnDie;

    public void JumpEnd()
    {
        IsOnJump = false;
    }

    public void DodgeEnd()
    {
        IsOnDodge = false;
    }

    public void AttackEnd()
    {
        IsOnAttack = false;
    }
}
