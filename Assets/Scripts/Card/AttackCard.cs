using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class AttackCard : MonoBehaviour
{
    private bool _isAttacking;
    private Vector2 _atkPos;

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

    public void Attack(HPCard cardHP, SpriteRenderer[] srs, TextMeshPro[] tmps)
    {
        Vector2 myPos = transform.position;
        _atkPos = cardHP.hitPos.position;
        _isAttacking = true;

        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOMove(_atkPos, 0.2f));
        seq.AppendCallback(() => AttackLogic(cardHP));
        seq.Join(transform.DOMove(myPos, 0.7f));
        seq.AppendCallback(() =>
        {
            _isAttacking = false;
            CardManager.Instanace.SetSiblingCard(-10, srs, tmps);
        });
    }

    public void AttackLogic(HPCard hp)
    {
        FeedbackManager.Instanace.MakeVFX(VFXType.smoke, _atkPos);
        FeedbackManager.Instanace.ShakeScreen();

        hp.DealDamage(10);
    }
}
