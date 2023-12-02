using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameBar : NetworkBehaviour
{
    public PlayerDie myDie;
    public PlayerDie enemyDie;

    [SerializeField] private RectTransform _myRect;
    [SerializeField] private RectTransform _enemtyRect;

    private NetworkVariable<int> _enemyValue = new NetworkVariable<int>(500);
    private NetworkVariable<int> _myValue = new NetworkVariable<int>(500);

    [SerializeField] private float _lerpSpeed;

    public void OnChangeGameBarValue(int newValue)
    {
        if(IsOwner)
        {
            _myValue.Value += newValue;
            _enemyValue.Value -= newValue;

            if(_enemyValue.Value == 0)
            {
                enemyDie.Die();
            }
        }
        else
        {
            _enemyValue.Value += newValue;
            _myValue.Value -= newValue;

            if (_myValue.Value == 0)
            {
                myDie.Die();
            }
        }
    }

    private void EnemyBarVisualChange()
    {
        if (_enemtyRect.sizeDelta.x == _enemyValue.Value) return;

        _enemtyRect.sizeDelta = Vector2.Lerp(_enemtyRect.sizeDelta, 
                                             new Vector2(_enemyValue.Value, _enemtyRect.sizeDelta.y), 
                                             Time.deltaTime * _lerpSpeed);
    }

    private void PlayerBarVisualChange()
    {
        if (_myRect.sizeDelta.x == _myValue.Value) return;

        _myRect.sizeDelta = Vector2.Lerp(_myRect.sizeDelta,
                                             new Vector2(_myValue.Value, _myRect.sizeDelta.y),
                                             Time.deltaTime * _lerpSpeed);
    }

    private void Update()
    {
        EnemyBarVisualChange();
        PlayerBarVisualChange();
    }
}
