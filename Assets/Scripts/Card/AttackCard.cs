using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class AttackCard : MonoBehaviour
{
    private bool _isAttacking;

    public void AttackReady(Transform trm)
    {
        if (_isAttacking) return;

        trm.DOScale(new Vector3(1.8756f, 2.1361f), 0.1f);
    }

    public void AttackCancle(Transform trm)
    {
        if (_isAttacking) return;

        trm.DOScale(new Vector3(1.8f, 2.05f), 0.1f);
    }

    public void Attack(Transform trm, Vector2 target, HPCard cardHP ,SpriteRenderer[] srs, TextMeshPro[] tmps)
    {
        Vector2 myPos = trm.position;
        _isAttacking = true;

        Sequence seq = DOTween.Sequence();
        seq.Append(trm.DOMove(target, 0.2f));
        seq.Append(trm.DOMove(myPos, 0.4f));
        seq.AppendCallback(() =>
        {
            _isAttacking = false;
            CardManager.Instanace.SetSiblingCard(-10, srs, tmps);
        });
    }
}
