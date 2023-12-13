using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Lobbies;
using UnityEngine;
using UnityEngine.SceneManagement;

public enum GameState
{
    Ready,
    Game,
    Win,
    Lose
}

public enum GameRole : ushort
{
    Host,
    Client
}

public class GameManager : NetworkBehaviour
{
    public static GameManager Instance;
    public event Action<GameState> GameStatehanged;
    private GameState _gameState;

    [SerializeField] private Transform _hostSpawnPos;
    [SerializeField] private Transform _clientSpawnPos;
    [SerializeField] private GameBar _gameBar;
    [SerializeField] private GameReady _gameReady;

    public NetworkList<GameData> players;

    public GameRole myGameRole;
    public ulong myClientID;
    public ulong winPlayerClientID;

    private bool _alreadyGameStart;

    [SerializeField] private Camera _mainCam;

    public Vector2 WorldToScreenPos(Vector3 worldPos)
    {
        return _mainCam.WorldToScreenPoint(worldPos);
    }

    public Vector2 ScreenToWorldPos(Vector3 screenPos)
    {
        return _mainCam.ScreenToWorldPoint(screenPos);
    }

    private void Awake()
    {
        Instance = this;
        players = new NetworkList<GameData>();
    }

    private void Start()
    {
        _gameState = GameState.Ready;
        Debug.Log($"Lobby Is Running! (LobbyID : {HostSingleton.Instnace.GamaManager.lobbyID})");

        NetworkManager.Singleton.OnClientStopped += HandleGoToMenu;
    }

    private void HandleGoToMenu(bool obj)
    {
        SceneManager.LoadScene(SceneList.MenuScene);
    }

    [ServerRpc(RequireOwnership = false)]
    public void GameExitServerRpc()
    {
        HostSingleton.Instnace.GamaManager.ShutdownAsync();
    }

    public override void OnNetworkSpawn()
    {
        if(IsHost)
        {
            HostSingleton.Instnace.GamaManager.OnPlayerConnect += OnPlayerConnectHandle;
            HostSingleton.Instnace.GamaManager.OnPlayerDisconnect += OnPlayerDisconnectHandle;
            myGameRole = GameRole.Host;

            var gameData = HostSingleton.Instnace.GamaManager.NetServer.GetUserDataByClientID(OwnerClientId);
            OnPlayerConnectHandle(gameData.userAuthID, OwnerClientId);
        }
        else
        {
            myGameRole = GameRole.Client;
        }
        
    }

    public override void OnNetworkDespawn()
    {
        if (IsHost)
        {
            HostSingleton.Instnace.GamaManager.OnPlayerConnect -= OnPlayerConnectHandle;
            HostSingleton.Instnace.GamaManager.OnPlayerDisconnect -= OnPlayerDisconnectHandle;
        }
    }

    private void OnPlayerConnectHandle(string authID, ulong clientID)
    {
        UserData data = HostSingleton.Instnace.GamaManager.NetServer.GetUserDataByClientID(clientID);
        
        players.Add(new GameData
        {
            clientID = clientID,
            playerName = data.name,
            ready = false,
        });
    }

    private void OnPlayerDisconnectHandle(string authID, ulong clientID)
    {
        foreach(GameData data in players)
        {
            if (data.clientID != clientID) continue;
            try
            {
                players.Remove(data);
            }
            catch
            {
                Debug.LogError($"{data.playerName} 삭제 중 오류 발생");
            }
            break;
        }
    }

    public void GameReady(bool isReady)
    {
        SendReadyStateServerRpc(NetworkManager.Singleton.LocalClientId, isReady);
        _alreadyGameStart = false;
    }

    [ServerRpc(RequireOwnership = false)]
    private void SendReadyStateServerRpc(ulong clientID, bool isReady)
    {
        for(int i = 0; i < players.Count; i++)
        {
            if(players[i].clientID == clientID)
            {
                GameData gd = players[i];
                players[i] = new GameData
                {
                    clientID = clientID,
                    playerName = gd.playerName,
                    ready = isReady,
                };
                break;
            }
        }
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        players?.Dispose();
    }

    public bool GameStart()
    {
        if (!IsHost || _alreadyGameStart || players.Count < 2) return false;

        for(int i = 0; i < players.Count; i++)
        {
            if (!players[i].ready) return false;
        }

        _alreadyGameStart = true;
        StartGameClientRpc();
        SpawnPlayers();
        GameConnectManager.Instance.GameSetAndGoClientRpc();
        return true;
    }

    
    

    private void SpawnPlayers()
    {
        foreach (var player in players)
        {
            if(player.clientID == OwnerClientId)
            {
                HostSingleton.Instnace.GamaManager.NetServer.SpawnPlayer
                (
                    player.clientID,
                    _hostSpawnPos.position
                );
            }
            else
            {
                HostSingleton.Instnace.GamaManager.NetServer.SpawnPlayer
                (
                    player.clientID,
                    _clientSpawnPos.position
                );
            }
        }
    }

    [ClientRpc]
    private void StartGameClientRpc()
    {
        _gameState = GameState.Game;
        GameStatehanged?.Invoke(_gameState);
    }
}
