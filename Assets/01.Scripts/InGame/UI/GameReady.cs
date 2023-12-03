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
        foreach(GameData gd in GameManager.Instance.players)
        {
            if(gd.clientID == OwnerClientId)
            {
                if(IsHost)
                {
                    UploadHostPanelServerRpc(gd.playerName.ToString());
                }
                else
                {
                    UploadClientPanelServerRpc(gd.playerName.ToString());
                }
            }
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
        Instantiate(_readyBtn, _hostPanel).transform.localPosition = _hostReadyBtnTrm.localPosition;
    }

    [ClientRpc]
    private void UploadClientPanelClientRpc(string name)
    {
        foreach(GameData gd in GameManager.Instance.players)
        {
            if(gd.clientID == OwnerClientId)
            {
                _clientName.text = name.ToString();
                _clientPanel.DOLocalMoveY(_clientAllocationPos.y, 0.5f);
            }
            else
            {
                _hostName.text = gd.playerName.ToString();
            }
        }

        if (IsHost) return;
        var btn = Instantiate(_readyBtn, _clientPanel);
        btn.transform.localPosition = _clientReadyBtnTrm.localPosition;
        btn.transform.rotation = Quaternion.identity;
    }
}
