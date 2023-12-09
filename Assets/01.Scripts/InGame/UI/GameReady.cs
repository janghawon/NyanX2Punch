using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using Unity.Netcode;

public enum Direction
{
    UP,
    DOWN,
    RIGHT,
    LEFT
}

public class GameReady : NetworkBehaviour
{
    [SerializeField] private Vector2 _clientReadyPos;
    [SerializeField] private Vector2 _clientAllocationPos;
    [SerializeField] private Vector2 _hostReadyPos;
    [SerializeField] private Vector2 _hostAllocationPos;

    [SerializeField] private Transform _hostPanel;
    [SerializeField] private Transform _clientPanel;

    [SerializeField] private TextMeshProUGUI _hostName;
    [SerializeField] private TextMeshProUGUI _clientName;

    [SerializeField] private TextMeshProUGUI[] _vsText;

    [SerializeField] private ReadyBtn _readyBtn;
    [SerializeField] private Transform _hostReadyBtnTrm;
    [SerializeField] private Transform _clientReadyBtnTrm;

    [SerializeField] private StartBtn _startBtn;
    [SerializeField] private ExitBtn _exitBtn;

    private void Awake()
    {
        _clientPanel.localPosition = _clientReadyPos;
    }

    public void ChangeMainText(string syntex)
    {
        for(int i = 0; i < _vsText.Length; i++)
        {
            _vsText[i].text = syntex;
        }
    }

    public void ChangeMainText(float scale, float duration)
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(_vsText[0].transform.DOScale(Vector2.one * scale, duration));
        seq.Join(_vsText[1].transform.DOScale(Vector2.one * scale, duration));
        seq.Join(_vsText[0].DOFade(0, duration));
        seq.Join(_vsText[1].DOFade(0, duration));
    }

    public void ResetMainText()
    {
        for (int i = 0; i < _vsText.Length; i++)
        {
            _vsText[i].text = "VS";
            _vsText[i].transform.localScale = Vector3.one;
            Color currentColor = _vsText[i].color;
            _vsText[i].color = new Color(currentColor.r, currentColor.g, currentColor.b, 1);
        }
    }

    [Tooltip("이 메서드의 Direction은 UP또는 DOWN만 사용 가능합니다.")]
    public void AllPanelMove(Direction dir, float duration)
    {
        if(dir == Direction.UP)
        {
            _hostPanel.DOLocalMove(_hostReadyPos, duration);
            _clientPanel.DOLocalMove(_clientReadyPos, duration);
        }
        else if(dir == Direction.DOWN)
        {
            _hostPanel.DOLocalMove(_hostAllocationPos, duration);
            _clientPanel.DOLocalMove(_clientAllocationPos, duration);
        }
    }

    private void Start()
    {
        PanelSetting();
    }

    public void PanelSetting()
    {
        if (IsHost)
        {
            UploadHostPanelServerRpc(GameManager.Instance.players[0].playerName.ToString());
        }
        else
        {
            UploadClientPanelServerRpc(GameManager.Instance.players[1].playerName.ToString());
        }
    }

    [ServerRpc(RequireOwnership = false)]
    public void UploadHostPanelServerRpc(string name)
    {
        UploadHostPanelClientRpc(name);
    }

    [ServerRpc(RequireOwnership = false)]
    public void UploadClientPanelServerRpc(string name)
    {
        UploadClientPanelClientRpc(name);
    }

    [ServerRpc(RequireOwnership = false)]
    public void RemoveClientPanelServerRpc()
    {
        _clientPanel.DOLocalMoveY(_clientReadyPos.y, 0.5f);
    }

    [ClientRpc]
    private void UploadHostPanelClientRpc(string name)
    {
        _hostName.text = name.ToString();
        _hostPanel.DOLocalMoveY(_hostAllocationPos.y, 0.5f);

        if (!IsHost) return;
        var btn = Instantiate(_readyBtn, _hostPanel);
        btn.transform.localPosition = _hostReadyBtnTrm.localPosition;

        btn.ReadyBtnSetting();

        Instantiate(_startBtn, _hostPanel).StartBtnSetting();
    }

    [ClientRpc]
    private void UploadClientPanelClientRpc(string name)
    {
        _clientName.text = name.ToString();
        _clientPanel.DOLocalMoveY(_clientAllocationPos.y, 0.5f);

        if (IsHost) return;
        _hostName.text = GameManager.Instance.players[0].playerName.ToString();

        var btn = Instantiate(_readyBtn, _clientPanel);
        btn.transform.localPosition = _clientReadyBtnTrm.localPosition;
        btn.transform.localRotation = Quaternion.identity;

        btn.ReadyBtnSetting();
    }
}
