using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class ReadyBtn : MonoBehaviour
{
    private ProfileReady _profileReady;
    [SerializeField] private Color _acceptColor;
    [SerializeField] private Color _cancleColor;

    public TextMeshProUGUI btnText;
    public TextMeshProUGUI nameText;
    private Button _thisBtn;
    private Image _img;

    private void Awake()
    {
        _thisBtn = GetComponent<Button>();
        _img = _thisBtn.GetComponent<Image>();
        _profileReady = GameObject.Find("Canvas/ReadyUI").GetComponent<ProfileReady>();
    }
    private void Start()
    {
        if(GameManager.Instance.players[0].ready)
        {
            _profileReady.SetReadyServerRpc(GameRole.Host, true);
        }
        _thisBtn.onClick.AddListener(HandleGameReady);
    }

    private void HandleGameReady()
    {
        GameManager.Instance.GameReady();
        _img.color = _cancleColor;
        btnText.text = "준비 해제?";
        _profileReady.SetReadyServerRpc(GameManager.Instance.myGameRole, true);

        _thisBtn.onClick.RemoveAllListeners();
        _thisBtn.onClick.AddListener(HandleGameReadyCancle);
    }

    private void HandleGameReadyCancle()
    {
        GameManager.Instance.GameReady();
        _img.color = _acceptColor;
        btnText.text = "준비 완료!";
        _profileReady.SetReadyServerRpc(GameManager.Instance.myGameRole, false);

        _thisBtn.onClick.RemoveAllListeners();
        _thisBtn.onClick.AddListener(HandleGameReady);
    }
}
