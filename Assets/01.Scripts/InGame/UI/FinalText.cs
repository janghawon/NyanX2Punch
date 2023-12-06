using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class FinalText : MonoBehaviour
{
    private void OnEnable()
    {
        transform.DOShakePosition(0.2f, 20, 20).SetLoops(-1);
    }
}
