using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;
using Unity.Netcode;
using System;

public class GameReady : NetworkBehaviour
{
    [SerializeField] private Vector2 _clientReadyPos;
    [SerializeField] private Vector2 _clientAllocationPos;
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

    private void Start()
    {
        if(IsHost)
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

    [ClientRpc]
    private void UploadHostPanelClientRpc(string name)
    {
        _hostName.text = name.ToString();
        var btn = Instantiate(_readyBtn, _hostPanel);
        btn.transform.localPosition = _hostReadyBtnTrm.localPosition;
        btn.btnText = btn.transform.Find("StateText").GetComponent<TextMeshProUGUI>();

        Instantiate(_startBtn, _hostPanel);
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
        btn.btnText = btn.transform.Find("StateText").GetComponent<TextMeshProUGUI>();
        btn.transform.rotation = Quaternion.identity;
    }
}
