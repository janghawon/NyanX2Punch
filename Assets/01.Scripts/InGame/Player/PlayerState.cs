using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public bool IsOnJump;
    public bool IsOnDodge;
    public bool IsOnAttack;
    public bool IsOnDie;

    public void AttackEnd()
    {
        IsOnAttack = false;
    }
}
