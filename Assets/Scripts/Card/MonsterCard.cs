using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DG.Tweening;
using UnityEngine.Events;

public class MonsterCard : CardBase
{
    public bool _canAttack;
    private AttackCard _attackCard;
    [SerializeField] private SpriteRenderer[] _srGroup;
    [SerializeField] private TextMeshPro[] _tmpGroup;

    [SerializeField] private UnityEvent<int, TextMeshPro> HandleHitEvent;

    public int currentATK;
    public int currentHP;
    public string cardName;
    public TribeType tribe;

    private void Start()
    {
        if(TryGetComponent<AttackCard>(out AttackCard a))
        {
            _attackCard = a;
            _canAttack = true;
        }

        Vector2 normalScalevalue = transform.localScale;
        transform.localScale = new Vector2(1.718f, 2.39f);
        transform.DOScale(normalScalevalue, 0.3f);
    }

    public void SetStatus(MonsterCardDataSO cardData)
    {
        currentATK = cardData.Atk;
        currentHP = cardData.HP;
        cardName = cardData.NAME;
        tribe = cardData.tribeType;
    }

    private void OnMouseDown()
    {
        if (!_canAttack || playerType == PlayerType.Enemy) return;

        if(!_isDragging)
        {
            SpawnArrow();
            _attackCard.AttackReady(transform);
            _isDragging = true;
        }
    }
    private void OnMouseUp()
    {
        if (!_canAttack || playerType == PlayerType.Enemy) return;

        Destroy(_arrowObj.gameObject);

        if (CardManager.Instanace.selectAtkCard != null)
        {
            if(CardManager.Instanace.selectAtkCard.TryGetComponent<HPCard>(out HPCard h))
            {
                CardManager.Instanace.SetSiblingCard(10, _srGroup, _tmpGroup);
                _attackCard.Attack(h, _srGroup, _tmpGroup);
            }
        }
        _attackCard.AttackCancle(transform);
        _isDragging = false;
    }
}
