using System;
using System.Collections;
using System.Collections.Generic;
using Unity.Netcode;
using UnityEngine;

public class GameConnectManager : NetworkBehaviour
{
    public static GameConnectManager Instance;
    [SerializeField] private GameReady _gameReady;
    [SerializeField] private int _readyTime;
    public List<PlayerMovement> playerMList = new List<PlayerMovement>();
    [SerializeField] private Transform _canTrm;
    [SerializeField] private GameEndPanel _endPanel;

    private void Awake()
    {
        Instance = this;
    }

    public void GameEndTurmSet(float sec)
    {
        StartCoroutine(GameEndTurmSetCo(sec));
    }

    IEnumerator GameEndTurmSetCo(float sec)
    {
        yield return new WaitForSeconds(sec);
        GameEndServerRpc();
    }

    [ServerRpc(RequireOwnership = false)]
    private void GameEndServerRpc()
    {
        UserData ud = HostSingleton.Instnace.GamaManager.NetServer.
                      GetUserDataByClientID(GameManager.Instance.winPlayerClientID);
        GameEndClientRpc(ud.name);
    }

    [ClientRpc]
    private void GameEndClientRpc(string name)
    {
        var panel = Instantiate(_endPanel, _canTrm);
        panel.winName = name;
    }

    [ServerRpc(RequireOwnership = false)]
    public void GameSetAndGoServerRpc()
    {
        GameSetAndGoClientRpc();
    }

    [ClientRpc]
    public void GameSetAndGoClientRpc()
    {
        _gameReady.AllPanelMove(Direction.UP, 0.5f);
        StartCoroutine(WaitTimeCo());
    }

    IEnumerator WaitTimeCo()
    {
        for(int i = _readyTime; i > 0; i--)
        {
            _gameReady.ChangeMainText(i.ToString());
            yield return new WaitForSeconds(1);
        }
        _gameReady.ChangeMainText("GO");
        yield return new WaitForSeconds(1);

        foreach(PlayerMovement pm in playerMList)
        {
            pm.canMovement = true;
        }
        
        _gameReady.ChangeMainText(3, 1f);
    }
}
