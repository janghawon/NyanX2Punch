using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardBase : MonoBehaviour
{
    [SerializeField] private CardAtkArrow _arrowPrefab;
    protected CardAtkArrow _arrowObj;
    private Transform _canvasTrm;
    protected bool _isDragging;
    public PlayerType playerType;

    private void Awake()
    {
        _canvasTrm = GameObject.Find("UICanvas").transform;
    }

    protected void SpawnArrow()
    {
        _arrowObj = Instantiate(_arrowPrefab, _canvasTrm);
        _arrowObj.hostCard = transform;
    }

    private void OnMouseEnter()
    {
        if(playerType == PlayerType.Enemy)
            CardManager.Instanace.selectAtkCard = this;
    }

    private void OnMouseExit()
    {
        if (playerType == PlayerType.Enemy)
            CardManager.Instanace.selectAtkCard = null;
    }
}
