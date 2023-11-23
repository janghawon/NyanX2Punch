using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public interface IAtackable : INormalable
{
    public void AttackReady(Transform trm)
    {
        trm.DOScale(new Vector3(1.8756f, 2.1361f), 0.1f);
    }

    public void AttackCancle(Transform trm)
    {
        trm.DOScale(new Vector3(1.8f, 2.05f), 0.1f);
    }

    public void Attack(Transform trm, Vector2 target)
    {
        Vector2 myPos = trm.position;

        Sequence seq = DOTween.Sequence();
        seq.Append(trm.DOMove(target, 0.4f));
        seq.Append(trm.DOMove(myPos, 0.4f));
    }
}
