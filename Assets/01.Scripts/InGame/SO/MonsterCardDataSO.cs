using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[CreateAssetMenu(menuName = "SO/MonsterCardData")]
public class MonsterCardDataSO : CardDataSO
{
    public TribeType tribeType;
    public int Atk;
    public int HP;
    public int COST;
    public string NAME;
    [Multiline(3)]
    public string INFO;
}
