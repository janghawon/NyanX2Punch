using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using Unity.Netcode;
using UnityEngine;

public class NetworkServer : IDisposable
{
    private NetworkManager _networkManager;
    public Action<string, ulong> OnClientJoin;
    public Action<string, ulong> OnClientLeft;

    private Dictionary<ulong, string> _clientToAuthDictionary = new ();
    private Dictionary<string, UserData> _authIdToUserDataDictionary = new ();

    private List<NetworkObject> _playerList = new List<NetworkObject>();

    public NetworkServer(NetworkManager nm)
    {
        _networkManager = nm;
        _networkManager.ConnectionApprovalCallback += ApprovalCheck;
        _networkManager.OnServerStarted += OnServerReady;
    }

    private void ApprovalCheck(NetworkManager.ConnectionApprovalRequest req, 
                               NetworkManager.ConnectionApprovalResponse res)
    {
        string json = Encoding.UTF8.GetString(req.Payload);
        UserData userData = JsonUtility.FromJson<UserData>(json);

        _clientToAuthDictionary[req.ClientNetworkId] = userData.userAuthID;
        _authIdToUserDataDictionary[userData.userAuthID] = userData;

        res.Approved = true;
        res.CreatePlayerObject = false;

        OnClientJoin?.Invoke(userData.userAuthID, req.ClientNetworkId);
    }

    private void OnServerReady()
    {
        _networkManager.OnClientDisconnectCallback += OnClientDisconnect;
    }

    private void OnClientDisconnect(ulong clientID)
    {
        if(_clientToAuthDictionary.TryGetValue(clientID, out var authID))
        {
            _clientToAuthDictionary.Remove(clientID);
            _authIdToUserDataDictionary.Remove(authID);
            OnClientLeft?.Invoke(authID, clientID);
        }
    }

    public UserData GetUserDataByClientID(ulong clientID)
    {
        if (_clientToAuthDictionary.TryGetValue(clientID, out string authID))
        {
            if (_authIdToUserDataDictionary.TryGetValue(authID, out UserData data))
            {
                return data;
            }
        }
        return null;
    }

    public UserData GetUserDataByAuthID(string authID)
    {
        if (_authIdToUserDataDictionary.TryGetValue(authID, out UserData data))
        {
            return data;
        }
        return null;
    }

    public void Dispose()
    {
        if (_networkManager == null) return;

        _networkManager.ConnectionApprovalCallback -= ApprovalCheck;
        _networkManager.OnServerStarted -= OnServerReady;
        _networkManager.OnClientDisconnectCallback -= OnClientDisconnect;

        HostSingleton.Instnace.GamaManager.ShutdownAsync();

        if (_networkManager.IsListening)
        {
            _networkManager.Shutdown();
        }
    }

    public void DestroyAllPlayer()
    {
        foreach(var p in _playerList)
        {
            GameObject.Destroy(p.gameObject);
        }
    }
}
