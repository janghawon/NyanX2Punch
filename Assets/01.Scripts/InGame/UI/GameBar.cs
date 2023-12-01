using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameBar : NetworkBehaviour
{
    [SerializeField] private RectTransform _myRect;
    [SerializeField] private RectTransform _enemtyRect;

    private int _enemyValue = 500;
    private int _myValue = 500;

    [SerializeField] private float _lerpSpeed;

    public void OnChangeGameBarValue(int newValue)
    {
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
    }

    private void EnemyBarVisualChange()
    {
        if (_enemtyRect.sizeDelta.x == _enemyValue) return;

        _enemtyRect.sizeDelta = Vector2.Lerp(_enemtyRect.sizeDelta, 
                                             new Vector2(_enemyValue, _enemtyRect.sizeDelta.y), 
                                             Time.deltaTime * _lerpSpeed);
    }

    private void PlayerBarVisualChange()
    {
        if (_myRect.sizeDelta.x == _myValue) return;

        _myRect.sizeDelta = Vector2.Lerp(_myRect.sizeDelta,
                                             new Vector2(_myValue, _myRect.sizeDelta.y),
                                             Time.deltaTime * _lerpSpeed);
    }

    private void Update()
    {
        EnemyBarVisualChange();
        PlayerBarVisualChange();
    }
}
