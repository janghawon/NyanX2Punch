using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class DesertSequence : NetworkBehaviour
{
    private Transform _canvas;
    [SerializeField] private Transform _hotText;
    [SerializeField] private PlayerState _playerState;
    private GameBar _gameBar;

    private float _currentTime = 0;
    private float _hotTime = 0;

    private void Awake()
    {
        _canvas = GameObject.Find("Canvas").transform;
        _gameBar = _canvas.transform.Find("EmptyBar").GetComponent<GameBar>();
    }

    private void FixedUpdate()
    {
        
    }
}
