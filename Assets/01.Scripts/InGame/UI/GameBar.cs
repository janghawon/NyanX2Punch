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

    private NetworkVariable<int> _clientValue = new NetworkVariable<int>(500);
    private NetworkVariable<int> _hostValue = new NetworkVariable<int>(500);

    [SerializeField] private float _lerpSpeed;

    [ServerRpc(RequireOwnership = false)]
    public void OnChangeGameBarValueServerRpc(int newValue, ulong clientID)
    {
        if (clientID == OwnerClientId)
        {
            _hostValue.Value += newValue;
            _clientValue.Value -= newValue;

            if (_clientValue.Value == 0)
            {
                enemyDie.Die();
            }
        }
        else
        {
            _clientValue.Value += newValue;
            _hostValue.Value -= newValue;

            if (_hostValue.Value == 0)
            {
                myDie.Die();
            }
        }
    }

    private void EnemyBarVisualChange()
    {
        if (_enemtyRect.sizeDelta.x == _clientValue.Value) return;

        _enemtyRect.sizeDelta = Vector2.Lerp(_enemtyRect.sizeDelta, 
                                             new Vector2(_clientValue.Value, _enemtyRect.sizeDelta.y), 
                                             Time.deltaTime * _lerpSpeed);
    }

    private void PlayerBarVisualChange()
    {
        if (_myRect.sizeDelta.x == _hostValue.Value) return;

        _myRect.sizeDelta = Vector2.Lerp(_myRect.sizeDelta,
                                             new Vector2(_hostValue.Value, _myRect.sizeDelta.y),
                                             Time.deltaTime * _lerpSpeed);
    }

    private void Update()
    {
        EnemyBarVisualChange();
        PlayerBarVisualChange();
    }
}
