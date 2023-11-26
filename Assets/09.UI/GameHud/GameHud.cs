using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.Netcode;
using UnityEngine;
using UnityEngine.UIElements;

public class GameHud : MonoBehaviour
{
    private UIDocument _uiDocument;
    private Button _startGameBtn;
    private Button _readyGameBtn;
    private VisualElement _waitingPanel;

    private Label _hostScoreTxt;
    private Label _clientScoreTxt;

    private VisualElement _resultBox;

    private List<PlayerUI> _players = new();

    private void Awake()
    {
        _uiDocument = GetComponent<UIDocument>();
    }

    private void OnEnable()
    {
        var root = _uiDocument.rootVisualElement;
        _startGameBtn = root.Q<Button>("btn-start");
        _readyGameBtn = root.Q<Button>("btn-ready");
        _waitingPanel = root.Q<VisualElement>("waiting-panel");

        _hostScoreTxt = root.Q<Label>("host-score");
        _clientScoreTxt = root.Q<Label>("client-score");
        _resultBox = root.Q<VisualElement>("result-box");
        _resultBox.AddToClassList("off");

        root.Query<VisualElement>(className: "player").ToList().ForEach(x =>
        {
            var player = new PlayerUI(x);
            _players.Add(player);
            player.RemovePlayerUI();
        });

        _startGameBtn.clicked += HandleGameStartClick;
        _readyGameBtn.clicked += HandleReadyClick;
        root.Q<Button>("btn-restart").clicked += HandleRestartClick;
        SignalHub.OnScoreChanged += HandleScoreChanged;
        SignalHub.OnEndGame += HandleEndGame;
    }

    private void HandleRestartClick()
    {
        _resultBox.AddToClassList("off");
        _waitingPanel.RemoveFromClassList("off");
        GameManager.Instance.GameReady();
    }

    private void HandleEndGame(bool isWin)
    {
        string msg = isWin ? "You Win!!" : "You Lose..";
        _resultBox.Q<Label>("result-label").text = msg;
        _resultBox.RemoveFromClassList("off");
    }

    private void HandleScoreChanged(int hostScore, int clientScore)
    {
        _hostScoreTxt.text = hostScore.ToString();
        _clientScoreTxt.text = clientScore.ToString();
    }

    private void Start()
    {
        GameManager.Instance.players.OnListChanged += HandlePlayerListChanged;
        GameManager.Instance.GameStatehanged += HandleGameStateChanged;

        foreach(GameData data in GameManager.Instance.players)
        {
            HandlePlayerListChanged(new NetworkListEvent<GameData>
            {
                Type = NetworkListEvent<GameData>.EventType.Add,
                Value = data
            });
        }
    }

    private void OnDestroy()
    {
        GameManager.Instance.players.OnListChanged -= HandlePlayerListChanged;
        GameManager.Instance.GameStatehanged -= HandleGameStateChanged;
    }

    private bool CheckPlayerExist(ulong clientID)
    {
        return _players.Any(x => x.clientID == clientID);
    }

    private PlayerUI FindEmptyPlayerUI()
    {
        foreach(var playerUI in _players)
        {
            if(playerUI.clientID == 999)
            {
                return playerUI;
            }
        }
        return null;
    }
    
    private void HandlePlayerListChanged(NetworkListEvent<GameData> evt)
    {
        Debug.Log($"{evt.Type}, {evt.Value.clientID}");
        switch (evt.Type)
        {
            case NetworkListEvent<GameData>.EventType.Add:
            {
                if (!CheckPlayerExist(evt.Value.clientID))
                {
                    var playerUI = FindEmptyPlayerUI();
                    playerUI.SetGameData(evt.Value);
                    playerUI.SetColor(GameManager.Instance.slimeColors[evt.Value.colorIdx]);
                    playerUI.VisiblePlayerUI();
                }
                break;
            }
            case NetworkListEvent<GameData>.EventType.Remove:
            {
                PlayerUI pu = _players.Find(x => x.clientID == evt.Value.clientID);
                    Debug.Log(pu);
                pu.RemovePlayerUI();
                break;
            }
            case NetworkListEvent<GameData>.EventType.Value:
            {
                    //플레이어 UI를 찾아서 Set쳌
                PlayerUI pu = _players.Find(x => x.clientID == evt.Value.clientID);
                pu.SetCheck(evt.Value.ready);
                break;
            }
        }
    }

    private void HandleGameStateChanged(GameState obj)
    {
        if(obj == GameState.Game)
        {
            _waitingPanel.AddToClassList("off");
        }
    }

    private void HandleGameStartClick()
    {
        if(GameManager.Instance.myGameRole != GameRole.Host)
        {
            Debug.Log("게임 호스트만 게임을 시작할 수 있습니다.");
            return;
        }

        GameManager.Instance.GameStart();
    }

    private void HandleReadyClick()
    {
        GameManager.Instance.GameReady();
    }
}
