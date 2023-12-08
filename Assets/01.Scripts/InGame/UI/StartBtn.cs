using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class StartBtn : MonoBehaviour
{
    private Button _thisBtn;

    private void Awake()
    {
        _thisBtn = GetComponent<Button>();
    }

    private void Start()
    {
        name = "GameStartBtn";
    }

    public void StartBtnSetting()
    {
        _thisBtn.onClick.AddListener(HandleGameStart);
    }

    private void HandleGameStart()
    {
        if(GameManager.Instance.GameStart())
            _thisBtn.onClick.RemoveAllListeners();
    }    
}
