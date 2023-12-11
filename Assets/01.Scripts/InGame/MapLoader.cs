using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;
using System;

public enum MapType
{
    desert,
    dirt,
    forest,
    java,
    snow,
    ocean
}

public class MapLoader : NetworkBehaviour
{
    [SerializeField] private GameObject[] _mapObjArr;
    private Dictionary<MapType, GameObject> _mapDic = new Dictionary<MapType, GameObject>();

    private void Awake()
    {
        int j = 0;
        foreach(MapType mt in Enum.GetValues(typeof(MapType)))
        {
            _mapDic.Add(mt, _mapObjArr[j]);
            j++;
        }

        for (int i = 0; i < _mapObjArr.Length; i++)
        {
            _mapObjArr[i].SetActive(false);
        }
    }
    public override void OnNetworkSpawn()
    {
        if(IsHost)
        {
            MapType mt = HostSingleton.Instnace.GamaManager.selectMapType;
            LoadMapClientRpc(mt);
        }
    }

    [ClientRpc]
    private void LoadMapClientRpc(MapType mt)
    {
        _mapDic[mt].SetActive(true);
    }
}
