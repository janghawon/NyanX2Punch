using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DrawManager : MonoBehaviour
{
    private static DrawManager _instance;
    public static DrawManager Instanace
    {
        get
        {
            if (_instance != null) return _instance;
            _instance = FindObjectOfType<DrawManager>();
            if (_instance == null)
            {
                Debug.LogError("Not Exist DrawManager");
            }
            return _instance;
        }
    }

    [SerializeField] private Hand _hand;
    [SerializeField] private HandCard _handCard;

    [SerializeField] private Transform _cardSpawnTrm;
    [SerializeField] private Transform _drawCenterPos;
    [SerializeField] private Transform _cardParent;
    [SerializeField] private float _centerWaitTime;

    private void Update()
    {
        if (Input.GetKeyDown(KeyCode.Space))
            DrawCard();
    }

    [ContextMenu("Draw")]
    public void DrawCard()
    {
        HandCard card = Instantiate(_handCard, _cardSpawnTrm.position, Quaternion.identity);
        card.transform.SetParent(_cardParent);
        _hand.playerHands.Add(card);
        card.DrawEvent(_drawCenterPos, _centerWaitTime);
    }

    public void GenerateCard()
    {
        _hand.GenerateCardPostion();
    }
}
