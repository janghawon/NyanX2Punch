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

    private void Awake()
    {
        Instance = this;
    }

    [ServerRpc(RequireOwnership = false)]
    public void GameSetAndGoServerRpc()
    {
        Debug.Log("이게 왜 실행이 안되는 것이야?");
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
