using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerState : MonoBehaviour
{
    public static bool IsOnJump;
    public static bool IsOnDodge;

    public void JumpEnd()
    {
        IsOnJump = false;
        Debug.Log(IsOnJump);
    }

    public void DodgeEnd()
    {
        IsOnDodge = false;
    }
}
