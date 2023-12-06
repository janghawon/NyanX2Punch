using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;
using UnityEngine.UI;
using TMPro;
using Unity.Services.Lobbies.Models;

public class Room
{
    public event Action<string> MakeLobbyBtnEvent;
    private bool _isLobbyRefresh = false;
    public event Action<Lobby> JoinRoomEvent;
    public event Action<Lobby> DestroyRoomEvent;

    private Transform _roomElement;
    private Transform _content;
    private Button _refreshBtn;

    public Room(Transform roomElement, Transform content, Button refreshBtn)
    {
        _roomElement = roomElement;
        _content = content;
        _refreshBtn = refreshBtn;

        _refreshBtn.onClick.AddListener(HandleRefreshBtnClick);
        HandleRefreshBtnClick();
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

        foreach(Transform exist in _content)
        {
            GameObject.Destroy(exist.gameObject);
        }

        foreach (var lobby in list)
        {
            Debug.Log(lobby);
            var lobyTemplate = GameObject.Instantiate(_roomElement, _content);

            lobyTemplate.Find("RoomName").GetComponent<TextMeshProUGUI>().text = lobby.Name;
            Button btn = lobyTemplate.Find("EnterBtn").GetComponent<Button>();
            btn.onClick.AddListener(() => JoinRoomEvent?.Invoke(lobby));
        }
        _isLobbyRefresh = false;
    }
}
