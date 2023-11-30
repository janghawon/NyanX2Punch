using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameBar : NetworkBehaviour
{
    [SerializeField] private RectTransform _myRect;
    [SerializeField] private RectTransform _enemtyRect;

    private int _enemyValue;
    private int _myValue;

    public void OnChangeGameBarValue(int newValue)
    {
        Debug.Log(1);
        if(IsOwner)
        {
            _myValue += newValue;
            _enemyValue -= newValue;
        }
        else
        {
            _enemyValue += newValue;
            _myValue -= newValue;
        }
        ValueBarVisualChange();
    }

    private void ValueBarVisualChange()
    {
        int fullSize = _enemyValue + _myValue;

        int enemyRectValue = _enemyValue / fullSize * 1000;
        int myRectValue = _myValue / fullSize * 1000;

        _enemtyRect.sizeDelta = new Vector2(enemyRectValue, _enemtyRect.sizeDelta.y);
        _myRect.sizeDelta = new Vector2(myRectValue, _myRect.sizeDelta.y);
    }
}
