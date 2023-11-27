using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
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

    [SerializeField] private Transform _spawnPosition;
    public Color[] slimeColors;

    public NetworkList<GameData> players;

    public GameRole myGameRole;

    private ushort _colorIdx = 0;

    private int _readyUserCount = 0;

    public TurnManager TurnManager { get; private set; }

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

        TurnManager = GetComponent<TurnManager>();
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

            var gameData = HostSingleton.Instnace.GamaManager.NetServer.GetUserDataByClientID(OwnerClientId);
            OnPlayerConnectHandle(gameData.userAuthID, OwnerClientId);
            myGameRole = GameRole.Host;
        }
        else
        {
            myGameRole = GameRole.Client;
        }
    }

    [ClientRpc]
    public void SendResultToClientRpc(GameRole winner)
    {
        if(winner == myGameRole)
        {
            _gameState = GameState.Win;
            SignalHub.OnEndGame?.Invoke(true);
        }
        else
        {
            _gameState = GameState.Lose;
            SignalHub.OnEndGame?.Invoke(false);
        }

        GameStatehanged?.Invoke(_gameState);
    }

    public void SendResultToClient(GameRole winner)
    {
        HostSingleton.Instnace.GamaManager.NetServer.DestroyAllPlayer();
        SendResultToClientRpc(winner);
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
            colorIdx = _colorIdx
        });
        ++_colorIdx;
    }

    private void OnPlayerDisconnectHandle(string authID, ulong clientID)
    {
        foreach(GameData data in players)
        {
            if (data.clientID != clientID) continue;
            try
            {
                players.Remove(data);
                --_colorIdx;
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
                    colorIdx = gd.colorIdx
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
        if (!IsHost) return;
        if(_readyUserCount == 2)
        {
            StartGameClientRpc();
            SceneManager.LoadScene(SceneList.GameScene);
        }
        else
        {
            Debug.Log("플레이어들이 모두 준비를 완료하여야 게임을 시작할 수 있습니다.");
        }
    }

    [ClientRpc]
    private void StartGameClientRpc()
    {
        _gameState = GameState.Game;
        GameStatehanged?.Invoke(_gameState);
    }

}
