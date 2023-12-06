using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FlyingCat : MonoBehaviour
{
    private void Start()
    {
        transform.DOShakePosition(0.5f, 0.3f, 4).SetLoops(-1);
    }
}
