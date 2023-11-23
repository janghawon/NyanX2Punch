using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class CardManager : MonoBehaviour
{
    private static CardManager _instance;
    public static CardManager Instanace
    {
        get
        {
            if (_instance != null) return _instance;
            _instance = FindObjectOfType<CardManager>();
            if (_instance == null)
            {
                Debug.LogError("Not Exist F_CardManager");
            }
            return _instance;
        }
    }
    public CardDataSO selectCard;
    public CardBase selectAtkCard;
    public Hand _hand;
    [SerializeField] private LayerMask _cardZoneMark;

    private void Awake()
    {
        _hand = GameObject.Find("MyHand").GetComponent<Hand>();
    }
    public CardZone OnMouseDetectZone(Vector3 pos)
    {
        RaycastHit2D hit = Physics2D.Raycast(pos, Vector2.zero, 0f, _cardZoneMark);

        if (hit.collider == null) return null;
        if (hit.collider.TryGetComponent<CardZone>(out CardZone cz))
        {
            if (cz.canEmplaceCard) return cz;
        }

        return null;
    }
    public void SetMonsterCard(CardZone cardZone)
    {
        Instantiate(selectCard.cardBase, cardZone.transform.position, Quaternion.identity);
        cardZone.canEmplaceCard = false;
    }
    public void UseCard(HandCard useCard)
    {
        _hand.playerHands.Remove(useCard);
        Destroy(useCard.gameObject);
        _hand.GenerateCardPostion();
    }
    public void SetSiblingCard(int addValue, SpriteRenderer[] srGroup, TextMeshPro[] tmpGroup)
    {
        foreach (SpriteRenderer sr in srGroup)
        {
            sr.sortingOrder += addValue;
        }
        foreach (TextMeshPro t in tmpGroup)
        {
            t.sortingOrder += addValue;
        }
    }
}
