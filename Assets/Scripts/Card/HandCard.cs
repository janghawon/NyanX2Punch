using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;
using TMPro;

public class HandCard : MonoBehaviour
{
    [SerializeField] private CardDataSO _myCardData;
    [SerializeField] private Vector2 _normalScaleValue;
    [SerializeField] private Vector2 _centerScalevalue;
    [SerializeField] private Vector2 _selectScaleValue;
    [SerializeField] private SpriteRenderer[] _srGroup = new SpriteRenderer[6];
    [SerializeField] private TextMeshPro[] _textGroup = new TextMeshPro[5];

    private Vector2 _myPos;
    private bool _canSkipWait;
    private bool _isInHand;
    private Coroutine waitCo;

    public void DrawEvent(Transform centerPos, float waitSecond)
    {
        CardManager.Instanace.SetSiblingCard(10, _srGroup, _textGroup);
        _canSkipWait = false;
        _isInHand = false;
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOMove(centerPos.position, 0.5f));
        seq.Join(transform.DOScale(_centerScalevalue, 0.5f));
        seq.AppendCallback(() =>
        {
            _canSkipWait = true;
            waitCo = StartCoroutine(WaitTime(waitSecond));
        });
    }
    IEnumerator WaitTime(float time)
    {
        yield return new WaitForSeconds(time);
        InnerHand();
    }
    private void InnerHand()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOScale(_normalScaleValue, 1f));
        seq.AppendCallback(() =>
        {
            CardManager.Instanace.SetSiblingCard(-10, _srGroup, _textGroup);
        });
        DrawManager.Instanace.GenerateCard();
    }

    public void OnMouseEnter()
    {
        if (!_isInHand) return;

        _myPos = transform.position;
        CardManager.Instanace.selectCard = _myCardData; // SO 만들고 교체
        CardManager.Instanace.SetSiblingCard(10, _srGroup, _textGroup);
        transform.DOScale(_selectScaleValue, 0.2f);
    }

    public void OnMouseDrag()
    {
        if (!_isInHand) return;

        transform.position = GameManager.Instanace.ScreenToWorldPos(Input.mousePosition);
    }

    public void OnMouseUp()
    {
        if (!_isInHand) return;

        CardZone cardZone = CardManager.Instanace.OnMouseDetectZone(transform.position);
        if(cardZone != null)
        {
            CardManager.Instanace.SetMonsterCard(cardZone);
            CardManager.Instanace.UseCard(this);
        }
        else
        {
            transform.position = _myPos;
        }
    }

    public void OnMouseExit()
    {
        if (!_isInHand) return;

        CardManager.Instanace.SetSiblingCard(-10, _srGroup, _textGroup);
        transform.DOScale(_normalScaleValue, 0.2f);
    }

    private void Update()
    {
        if(_canSkipWait && Input.GetMouseButtonDown(0))
        {
            StopCoroutine(waitCo);
            InnerHand();
            _canSkipWait = false;
            _isInHand = true;
        }
    }
}
