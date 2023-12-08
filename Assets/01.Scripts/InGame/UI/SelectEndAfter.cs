using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using Unity.Netcode;

public class SelectEndAfter : NetworkBehaviour
{
    [SerializeField] private Image _blackPanel;
    [SerializeField] private Transform _selectPanel;
    private GameEndPanel _gameEndPanel;

    private void Awake()
    {
        _blackPanel.color = new Color(0, 0, 0, 0);
        _gameEndPanel = GameObject.Find("Canvas/GameEndPanel").GetComponent<GameEndPanel>();
    }

    private void Start()
    {
        DrawPanel();
    }

    public void DrawPanel()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(_selectPanel.DOLocalMoveY(0, 0.3f));
        seq.Join(_blackPanel.DOFade(1, 0.3f));
    }

    public void ExitPanel()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(_selectPanel.DOLocalMoveY(-250, 0.3f));
        seq.Join(_blackPanel.DOFade(0, 0.3f));
        seq.AppendCallback(() =>
        {
            _gameEndPanel.IsOnPanel = false;
            Destroy(_gameEndPanel.gameObject);
            Destroy(gameObject);
        });
    }

    public void ExitGame()
    {
        GameManager.Instance.GameExit(OwnerClientId);
    }

    public void GameSet()
    {
        GameConnectManager.Instance.ResetGame();
    }
}
