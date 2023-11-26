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
    public Transform impactPos;
    [SerializeField] private Transform _cardVisual;
    [SerializeField] private SpriteRenderer[] _srGroup;
    [SerializeField] private TextMeshPro[] _tmpGroup;

    [SerializeField] private UnityEvent<int, int, TextMeshPro, bool> HandleHPChangeEvent;
    [SerializeField] private UnityEvent<int, int, TextMeshPro> HandleATKChangeEvent;

    private int _currentAtk;
    private int _currentHP;

    public int currentATK
    {
        get
        {
            return _currentAtk;
        }
        set
        {
            _currentAtk = value;
            _tmpGroup[0].text = _currentAtk.ToString();
        }
    }
    public int currentHP
    {
        get
        {
            return _currentHP;
        }
        set
        {
            _currentHP = value;
            _tmpGroup[1].text = _currentHP.ToString();
        }
    }
    public string cardName;
    public TribeType tribe;

    [HideInInspector]
    public MonsterCardDataSO myData;

    private void Start()
    {
        if(TryGetComponent<AttackCard>(out AttackCard a))
        {
            _attackCard = a;
            _canAttack = true;
        }

        Vector2 normalScalevalue = _cardVisual.localScale;
        _cardVisual.localScale = new Vector2(1.718f, 2.39f);
        _cardVisual.DOScale(normalScalevalue, 0.3f);

        myData = (MonsterCardDataSO)CardManager.Instanace.selectCard;
        SetStatus(myData);
    }

    public void SetStatus(MonsterCardDataSO cardData)
    {
        currentATK = cardData.Atk;
        currentHP = cardData.HP;
    }

    public void HPChange(int prev, int value, bool isMine)
    {
        HandleHPChangeEvent?.Invoke(prev, value, _tmpGroup[1], isMine);
    }

    public void ATKChange(int prev, int value)
    {
        HandleATKChangeEvent?.Invoke(prev, value, _tmpGroup[0]);
    }

    private void OnMouseDown()
    {
        if (!_canAttack || playerType == PlayerType.Enemy) return;

        if(!_isDragging)
        {
            SpawnArrow();
            _attackCard.AttackReady(_cardVisual);
            _isDragging = true;
        }
    }
    private void OnMouseUp()
    {
        if (!_canAttack || playerType == PlayerType.Enemy) return;

        Destroy(_arrowObj.gameObject);

        if (CardManager.Instanace.selectAtkCard != null)
        {
            if(CardManager.Instanace.selectAtkCard.TryGetComponent<MonsterCard>(out MonsterCard h))
            {
                CardManager.Instanace.SetSiblingCard(10, _srGroup, _tmpGroup);
                _attackCard.Attack(h, _srGroup, _tmpGroup);
            }
        }
        _attackCard.AttackCancle(_cardVisual);
        _isDragging = false;
    }
}
