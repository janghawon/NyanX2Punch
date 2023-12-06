using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameBar : NetworkBehaviour
{
    [SerializeField] private RectTransform _myRect;
    [SerializeField] private RectTransform _enemtyRect;

    public NetworkVariable<int> clientValue = new NetworkVariable<int>(500);
    public NetworkVariable<int> hostValue = new NetworkVariable<int>(500);

    [SerializeField] private float _lerpSpeed;

    private bool _isGameEnd;

    [ServerRpc(RequireOwnership = false)]
    public void OnChangeGameBarValueServerRpc(int newValue, ulong clientID)
    {
        if (_isGameEnd) return;

        if(clientID == OwnerClientId)
        {
            hostValue.Value += newValue;
            clientValue.Value -= newValue;
        }
        else
        {
            hostValue.Value -= newValue;
            clientValue.Value += newValue;
        }

        if(hostValue.Value == 0 || clientValue.Value == 0)
        {
            _isGameEnd = true;
        }
    }

    private void EnemyBarVisualChange()
    {
        if (_enemtyRect.sizeDelta.x == clientValue.Value) return;

        _enemtyRect.sizeDelta = Vector2.Lerp(_enemtyRect.sizeDelta, 
                                             new Vector2(clientValue.Value, _enemtyRect.sizeDelta.y), 
                                             Time.deltaTime * _lerpSpeed);
    }

    private void PlayerBarVisualChange()
    {
        if (_myRect.sizeDelta.x == hostValue.Value) return;

        _myRect.sizeDelta = Vector2.Lerp(_myRect.sizeDelta,
                                             new Vector2(hostValue.Value, _myRect.sizeDelta.y),
                                             Time.deltaTime * _lerpSpeed);
    }

    private void Update()
    {
        EnemyBarVisualChange();
        PlayerBarVisualChange();
    }
}
