using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class PlayerHealth : NetworkBehaviour
{
    public Transform hitTrm;
    [SerializeField] private PlayerDie _pDie;
    private GameBar _gamebar;

    private void Awake()
    {
        _gamebar = GameObject.Find("Canvas/EmptyBar").GetComponent<GameBar>();
    }

    public void Hit(Vector3 dir)
    {
        if (IsHost)
        {
            if(_gamebar.clientValue.Value == 0)
            {
                _pDie.DieServerRpc(IsHost, dir);
            }
        }
        else
        {
            if(_gamebar.hostValue.Value == 0)
            {
                _pDie.DieServerRpc(IsHost, dir);
            }
        }
    }
}
