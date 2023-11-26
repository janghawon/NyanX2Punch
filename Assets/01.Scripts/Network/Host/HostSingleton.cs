using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class HostSingleton : MonoBehaviour
{
    private static HostSingleton _instance;
    public static HostSingleton Instnace
    {
        get
        {
            if (_instance != null) return _instance; 
            _instance = FindObjectOfType<HostSingleton>();
            if(_instance == null)
            {
                Debug.LogError("No host singletone");
            }
            return _instance;
        }
    }
    
    public HostGameManager GamaManager { get; private set; }

    public void CreateHost()
    {
        GamaManager = new HostGameManager();
    }
}
