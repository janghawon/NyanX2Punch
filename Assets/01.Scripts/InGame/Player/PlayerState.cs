using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public static bool IsOnJump;
    public static bool IsOnDodge;
    public static bool IsOnAttack;

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
