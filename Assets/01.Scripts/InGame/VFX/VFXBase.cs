using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class VFXBase : MonoBehaviour
{
    protected Animator _animator;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
    }

    public void DestroyThisVFX()
    {
        Destroy(gameObject);
    }
}
