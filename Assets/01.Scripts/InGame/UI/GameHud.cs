using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class GameHud : MonoBehaviour
{
    [SerializeField] private Button _startGameBtn;
    [SerializeField] private Button _readyGameBtn;

    private void Start()
    {
        _startGameBtn.onClick.AddListener(HandleGameStartClick);
        _readyGameBtn.onClick.AddListener(HandleGameReadyClick);
    }

    private void HandleGameStartClick()
    {
        if (GameManager.Instance.myGameRole != GameRole.Host)
        {
            Debug.Log("게임 호스트만 게임을 시작할 수 있습니다.");
            return;
        }

        GameManager.Instance.GameStart();
    }

    private void HandleGameReadyClick()
    {
        GameManager.Instance.GameReady();
    }
}
