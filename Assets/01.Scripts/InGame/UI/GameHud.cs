using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameHud : MonoBehaviour
{
    private bool _isGameStart;
    [SerializeField] private Button _startGameBtn;
    [SerializeField] private Button _readyGameBtn;

    private void HandleGameStartClick()
    {
        if (GameManager.Instance.myGameRole != GameRole.Host || _isGameStart)
        {
            Debug.Log("���� ȣ��Ʈ�� ������ ������ �� �ֽ��ϴ�.");
            return;
        }
        _isGameStart = true;
        GameManager.Instance.GameStart();
    }

    private void HandleGameReadyClick()
    {
        GameManager.Instance.GameReady();
    }
}
