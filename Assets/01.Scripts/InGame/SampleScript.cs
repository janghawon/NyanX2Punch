using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SampleScript : MonoBehaviour
{
    [SerializeField] private CardZone _cardZone;
    [SerializeField] private CardDataSO _myCardData;

    private void Start()
    {
        CardManager.Instanace.SetMonsterCard(_cardZone, PlayerType.Enemy);
    }
}
