using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ReadyBtn : MonoBehaviour
{
    [SerializeField] private Color _acceptColor;
    [SerializeField] private Color _cancleColor;

    public TextMeshProUGUI _text;
    private Button _thisBtn;
    private Image _img;

    private void Awake()
    {
        _thisBtn = GetComponent<Button>();
        _img = _thisBtn.GetComponent<Image>();
    }
    private void Start()
    {
        _thisBtn.onClick.AddListener(HandleGameReady) ;
    }

    private void HandleGameReady()
    {
        GameManager.Instance.GameReady();
        _img.color = _cancleColor;
        _text.text = "준비 해제";

        _thisBtn.onClick.RemoveAllListeners();
        _thisBtn.onClick.AddListener(HandleGameReadyCancle);
    }

    private void HandleGameReadyCancle()
    {
        GameManager.Instance.GameReady();
        _img.color = _cancleColor;
        _text.text = "준비 해제";

        _thisBtn.onClick.RemoveAllListeners();
        _thisBtn.onClick.AddListener(HandleGameReady);
    }
}
