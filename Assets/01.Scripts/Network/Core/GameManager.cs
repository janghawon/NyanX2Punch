using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using Unity.Netcode;
using UnityEngine;

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

    public NetworkList<GameData> players;

    public GameRole myGameRole;
    public ulong winPlayerClientID;

    private bool _alreadyGameStart;

    private int _readyUserCount = 0;

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

    public void GameReady()
    {
        SendReadyStateServerRpc(NetworkManager.Singleton.LocalClientId);
    }

    [ServerRpc(RequireOwnership = false)]
    private void SendReadyStateServerRpc(ulong clientID)
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
                    ready = !gd.ready,
                };

                _readyUserCount += players[i].ready ? 1 : -1;
                break;
            }
        }
        
    }

    public override void OnDestroy()
    {
        base.OnDestroy();
        players?.Dispose();
    }

    public void GameStart()
    {
        if (!IsHost || _alreadyGameStart) return;
        if (_readyUserCount >= 1)
        {
            _alreadyGameStart = true;
            StartGameClientRpc();
            SpawnPlayers();
            GameConnectManager.Instance.GameSetAndGoClientRpc();
        }
        else
        {
            Debug.Log("플레이어들이 모두 준비를 완료하여야 게임을 시작할 수 있습니다.");
        }
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
