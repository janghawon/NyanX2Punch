using System;
using System.Collections;
using System.Collections.Generic;
using System.Text;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Netcode.Transports.UTP;
using Unity.Networking.Transport.Relay;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Relay;
using Unity.Services.Relay.Models;
using UnityEngine;

public class HostGameManager : IDisposable
{
    public NetworkServer NetServer { get; private set; }
    private Allocation _allocation;
    private string _joinCode;
    private string _lobbyID;
    private const int _maxConnections = 2;

    public event Action<string, ulong> OnPlayerConnect;
    public event Action<string, ulong> OnPlayerDisconnect;

    public HostGameManager()
    {
    }

    public async Task<bool> StartHostAsync(string lobbyname, UserData userData) 
    {
        try
        {
            _allocation = await Relay.Instance.CreateAllocationAsync(_maxConnections);
            _joinCode = await Relay.Instance.GetJoinCodeAsync(_allocation.AllocationId);

            var transport = NetworkManager.Singleton.GetComponent<UnityTransport>();
            var relayServerData = new RelayServerData(_allocation, "dtls");
            transport.SetRelayServerData(relayServerData);

            CreateLobbyOptions lobbyOptions = new CreateLobbyOptions();
            lobbyOptions.Data = new Dictionary<string, DataObject>()
            {
                {
                    "JoinCode", new DataObject(visibility: DataObject.VisibilityOptions.Member, value: _joinCode)
                }
            };
            Lobby lobby = await Lobbies.Instance.CreateLobbyAsync(lobbyname, _maxConnections, lobbyOptions);
            _lobbyID = lobby.Id;
            HostSingleton.Instnace.StartCoroutine(HeartBeatLobby(15));

            NetServer = new NetworkServer(NetworkManager.Singleton);
            NetServer.OnClientJoin += HandleClientJoin;
            NetServer.OnClientLeft += HandleClinetLeft;

            string userJson = JsonUtility.ToJson(userData);
            NetworkManager.Singleton.NetworkConfig.ConnectionData = Encoding.UTF8.GetBytes(userJson);

            NetworkManager.Singleton.StartHost();
            return true;
        }
        catch (Exception e)
        {
            Debug.LogError(e.Message);
            return false;
        }
    }

    private void HandleClinetLeft(string authID, ulong clientID)
    {
        OnPlayerDisconnect?.Invoke(authID, clientID);
    }

    private void HandleClientJoin(string authID, ulong clientID)
    {
        OnPlayerConnect?.Invoke(authID, clientID);
    }

    public void Dispose()
    {
        ShutDownAsync();
    }

    public async void ShutdownAsync()
    {
        if(!string.IsNullOrEmpty(_lobbyID))
        {
            if(HostSingleton.Instnace != null)
            {
                HostSingleton.Instnace.StopCoroutine(nameof(HeartBeatLobby));
            }

            try
            {
                await Lobbies.Instance.DeleteLobbyAsync(_lobbyID);
            }
            catch(LobbyServiceException ex)
            {
                Debug.LogError(ex);
            }
        }

        NetServer.OnClientLeft -= HandleClinetLeft;
        NetServer.OnClientJoin -= HandleClientJoin;
        _lobbyID = string.Empty;
        NetServer?.Dispose();
    }

    public async void ShutDownAsync()
    {
        NetServer.OnClientLeft -= HandleClinetLeft;
        NetServer.OnClientJoin -= HandleClientJoin;
        _lobbyID = string.Empty;
        NetServer?.Dispose();
    }

    private IEnumerator HeartBeatLobby(float time)
    {
        var timer = new WaitForSecondsRealtime(time);
        while(true)
        {
            Lobbies.Instance.SendHeartbeatPingAsync(_lobbyID);
            yield return timer;
        }
    }
}
