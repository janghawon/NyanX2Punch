using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class ClientSingleton : MonoBehaviour
{
    public ClientGamaManager GamaManager { get; private set; }

    private static ClientSingleton _instance;
    public static ClientSingleton Instance
    {
        get
        {
            if (_instance != null) return _instance;
            _instance = FindObjectOfType<ClientSingleton>();

            if(_instance == null)
            {
                Debug.LogError("No Client Singleton");
            }

            return _instance;
        }
    }

    public void CreateClient()
    {
        GamaManager = new ClientGamaManager(NetworkManager.Singleton);
    }

    private void OnDestroy()
    {
        
    }
}
