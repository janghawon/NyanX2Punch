using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class MonsterCard : CardBase
{
    public bool _canAttack;
    private AttackCard _attackCard;
    [SerializeField] private SpriteRenderer[] _srGroup;
    [SerializeField] private TextMeshPro[] _tmpGroup;

    private void Start()
    {
        if(TryGetComponent<AttackCard>(out AttackCard a))
        {
            _attackCard = a;
            _canAttack = true;
        }
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
                _attackCard.Attack(transform, h.transform.position, h, _srGroup, _tmpGroup);
            }
        }
        _attackCard.AttackCancle(transform);
        _isDragging = false;
    }
}
