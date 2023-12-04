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
        _thisBtn.onClick.AddListener(HandleGameStart);
    }

    private void HandleGameStart()
    {
        GameManager.Instance.GameStart();
    }    
}
