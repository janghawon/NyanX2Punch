using System;
using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Services.Authentication;
using Unity.Services.Core;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.SceneManagement;

public class ApplicationController : MonoBehaviour
{
    [SerializeField] private ClientSingleton _clientPrefab;
    [SerializeField] private HostSingleton _hostPrefab;
    [SerializeField] private NetworkObject _playerPrefab;

    public static event Action<string> OnMessageEvent;
    public static ApplicationController Instance;
    [SerializeField] private TitleScene _title;

    private void Awake()
    {
        Instance = this;
    }

    private async void Start()
    {
        DontDestroyOnLoad(gameObject);
        Application.targetFrameRate = 60;

        OnMessageEvent?.Invoke("게임 서비스 초기화 진행 중 . . .");
        await UnityServices.InitializeAsync();

        OnMessageEvent?.Invoke("네트워크 서비스 인증 중...");
        AuthenticationWrapper.OnMessageEvent += HandleAuthMessage;
        var state = await AuthenticationWrapper.DoAuth(3);

        if(state != AuthState.Authenticated)
        {
            OnMessageEvent?.Invoke("앱 인증 중 오류 발생, 앱을 다시 시작해주세용"); 
            return;
        }

        HostSingleton host = Instantiate(_hostPrefab, transform);
        host.CreateHost(_playerPrefab);

        ClientSingleton client = Instantiate(_clientPrefab, transform);
        client.CreateClient();

        _title.CompleteReady();
    }

    public async Task<List<Lobby>> GetLobbyList()
    {
        try
        {
            QueryLobbiesOptions options = new QueryLobbiesOptions();
            options.Count = 20;
            options.Filters = new List<QueryFilter>()
            {
                new QueryFilter(
                    field: QueryFilter.FieldOptions.AvailableSlots,
                    op: QueryFilter.OpOptions.GT,
                    value: "0"),
                new QueryFilter(
                    field: QueryFilter.FieldOptions.IsLocked,
                    op: QueryFilter.OpOptions.EQ,
                    value: "0"),
            };

            QueryResponse lobbies = await Lobbies.Instance.QueryLobbiesAsync(options);
            return lobbies.Results;
        }
        catch (LobbyServiceException ex)
        {
            Debug.LogError(ex);
            return new List<Lobby>();
        }
    }

    private void HandleAuthMessage(string msg)
    {
        OnMessageEvent?.Invoke(msg);
    }

    private void OnDestroy()
    {
        AuthenticationWrapper.OnMessageEvent -= HandleAuthMessage;
    }

    public async Task<bool> StartHostAsync(string username, string lobbyName)
    {
        var userData = new UserData
        {
            name = username,
            userAuthID = AuthenticationService.Instance.PlayerId
        };
        return await HostSingleton.Instnace.GamaManager.StartHostAsync(lobbyName, userData);
    }

    public async Task StartClientAsync(string usename, string joincode)
    {
        await ClientSingleton.Instance.GamaManager.StartClientAsync(joincode, GetUserData(usename));
    }

    private UserData GetUserData(string username)
    {
        return new UserData
        {
            name = username,
            userAuthID = AuthenticationService.Instance.PlayerId
        };
    }
}
