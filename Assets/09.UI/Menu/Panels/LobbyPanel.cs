using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UIElements;
using System;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using Unity.Services.Authentication;
using System.Threading.Tasks;

public class LobbyPanel
{
    private VisualElement _root;

    public event Action<string> MakeLobbyBtnEvent;

    private Label _statusLabel;
    public Label StatusLabel => _statusLabel;

    private VisualTreeAsset _lobbyItemAsset;
    private ScrollView _lobbyScrollView;
    private bool _isLobbyRefresh = false;
    private bool _isJoinning = false;
    public event Action<Lobby> JoinLobbyEvent;

    public LobbyPanel(VisualElement root, VisualTreeAsset lobbyAsset)
    {
        _root = root;

        _statusLabel = root.Q<Label>("status-label");
        _lobbyScrollView = root.Q<ScrollView>("lobby-scroll");
        _lobbyItemAsset = lobbyAsset;
        root.Q<Button>("btn-refresh").clicked += HandleRefreshBtnClick;
    }

    private void HandleRefreshBtnClick()
    {
        Refresh();
    }

    public async void Refresh()
    {
        if (_isLobbyRefresh) return;
        _isLobbyRefresh = true;

        var list = await ApplicationController.Instance.GetLobbyList();

        foreach (var lobby in list)
        {
            var lobyTemplate = _lobbyItemAsset.Instantiate();
            _lobbyScrollView.Add(lobyTemplate);

            lobyTemplate.Q<Label>("lobby-name").text = lobby.Name;
            lobyTemplate.Q<Button>("btn-join").RegisterCallback<ClickEvent>(evt =>
            {
                JoinLobbyEvent?.Invoke(lobby);
            });
        }
        _isLobbyRefresh = false;
    }

    

    public void SetStatusText(string text)
    {
        _statusLabel.text = text;
    }
}
