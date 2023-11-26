using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;

public class HPCard : MonoBehaviour
{
    [SerializeField] private MonsterCard _monsterCard;
    [SerializeField] private Color _hitColor;
    [SerializeField] private SpriteRenderer _cardFilter;
    [SerializeField] private DamageImpact _damageImpact;

    public void HPChange(int prev, int value, TextMeshPro hpText, bool isMine)
    {
        int hpValue = prev - value;
        hpText.text = hpValue.ToString();

        if((hpValue) < prev)
        {
            hpText.color = _hitColor;
            Instantiate(_damageImpact, transform.position, Quaternion.identity).SetDamage(-value);
        }
        else
        {
            hpText.color = Color.white;
        }

        if(hpValue <= 0)
        {
            DestroyCard(isMine);
        }
    }

    private void DestroyCard(bool isMine)
    {
        _cardFilter.enabled = true;
        Sequence seq = DOTween.Sequence();
        
        if (!isMine)
            seq.Append(transform.DOShakePosition(0.4f, 0.2f, 20));
        else
            seq.Append(transform.DOScale(Vector3.one, 1f));

        seq.AppendCallback(() =>
        {
            FeedbackManager.Instanace.MakeVFX(VFXType.Destroy, transform.position);
            CardManager.Instanace.OnDestroyCardList.Add(_monsterCard.myData);
            Destroy(gameObject);
        });
    }
}
