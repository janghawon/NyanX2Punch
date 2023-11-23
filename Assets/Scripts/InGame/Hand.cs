using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening;

public class Hand : MonoBehaviour
{
    public List<HandCard> playerHands = new List<HandCard>();
    [SerializeField] private float _easingTime;
    //�� �� = 0, �� �� = -1, 1, �� �� = -2, 0, 2, �� �� = -3 - 1, 1, 3

    public void GenerateCardPostion()
    {
        int haveCards = playerHands.Count;
        int startXpos = -(haveCards - 1);

        for(int i = 0; i < playerHands.Count; i++)
        {
            Vector2 targetPos = new Vector2(startXpos + i * 2, transform.position.y);
            playerHands[i].transform.DOMove(targetPos, _easingTime);
        }
    }
}
