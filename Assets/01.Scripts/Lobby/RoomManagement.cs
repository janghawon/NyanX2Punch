using System.Collections;
using System.Collections.Generic;
using System.Threading.Tasks;
using Unity.Netcode;
using Unity.Services.Lobbies;
using Unity.Services.Lobbies.Models;
using UnityEngine;
using UnityEngine.SceneManagement;
using TMPro;
using System;
using UnityEngine.UI;

public class RoomManagement : MonoBehaviour
{
    private bool _isWaiting = false;
    public const string nameKey = "userName";

    [SerializeField] private Transform _canvasTrm;
    [SerializeField] private MakeRoomPanel _createRoomPanel;
    [SerializeField] private ModifyPlayerNamePanel _modifyPNamePanel;
    [SerializeField] private TextMeshProUGUI _playerNameTxt;
    [SerializeField] private Transform _roomElement;
    [SerializeField] private Transform _content;
    [SerializeField] private Button _refreshBtn;

    private Room _myRoom;

    private void Start()
    {
        RefreshPlayerName();

        _myRoom = new Room(_roomElement, _content, _refreshBtn);
        _myRoom.JoinRoomEvent += HandleJoinToRoom;

    }

    private async void HandleJoinToRoom(Lobby room)
    {
        if (_isWaiting) return;
        _isWaiting = true;
        try
        {
            Lobby joiningLobby = await Lobbies.Instance.JoinLobbyByIdAsync(room.Id);
            Debug.Log(joiningLobby);
            string JoinCode = joiningLobby.Data["JoinCode"].Value;
            Debug.Log(JoinCode);
            await ApplicationController.Instance.StartClientAsync(PlayerPrefs.GetString(RoomManagement.nameKey), JoinCode);
        }
        catch (LobbyServiceException ex)
        {
            Debug.LogError(ex);
        }
        finally
        {
            _isWaiting = false;
        }
    }

    public void RefreshPlayerName()
    {
        string userName = PlayerPrefs.GetString(nameKey, "username");
        _playerNameTxt.text = userName;
    }
    public void CreateMakeRoomPanel()
    {
        Instantiate(_createRoomPanel, _canvasTrm).roomManageMent = this;
    }
    public void ModifyPlayerName()
    {
        Instantiate(_modifyPNamePanel, _canvasTrm).roomManageMent = this;
    }
    public async void HandleCreateLobby(string roomName, Action panelEndFunc, TextMeshProUGUI syntex)
    {
        if (_isWaiting) return;
        _isWaiting = true;

        string username = PlayerPrefs.GetString(nameKey);
        bool result = await ApplicationController.Instance.StartHostAsync(username, roomName);

        if (result)
        {
            //NetworkManager.Singleton.SceneManager.LoadScene(SceneList.GameScene, LoadSceneMode.Single);
            _myRoom.Refresh();
            panelEndFunc?.Invoke();
        }
        else
        {
            syntex.text = "룸 개설에 실패했습니다.";
            syntex.color = Color.red;
        }

        _isWaiting = false;
    }
}
