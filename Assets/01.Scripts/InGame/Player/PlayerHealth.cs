using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using System;

public class PlayerHealth : NetworkBehaviour
{
    public Transform hitTrm;
    [SerializeField] private PlayerDie _pDie;
    private GameBar _gamebar;

    public Action<bool, Vector3> _clientDieAction;
    public Action<bool, Vector3> _hostDieAction;

    private void Awake()
    {
        _gamebar = GameObject.Find("Canvas/EmptyBar").GetComponent<GameBar>();
    }

    public void Hit(Vector3 dir)
    {
        if (IsHost)
        {
            _clientDieAction += _pDie.DieServerRpc;
            if(_gamebar.clientValue.Value == 0)
            {
                _pDie.DieServerRpc(IsHost, dir);
            }
        }
        else
        {
            _hostDieAction += _pDie.DieClientRpc;
            if(_gamebar.hostValue.Value == 100)
            {
                _pDie.DieServerRpc(IsHost, dir);
            }
        }
    }
}
