using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using DG.Tweening;

public class GameEndPanel : MonoBehaviour
{
    private string _winName;
    public string winName
    {
        get
        {
            return _winName;
        }
        set
        {
            _winName = value;
            _nickText.text = _winName;
        }
    }
    [SerializeField] private TextMeshProUGUI _nickText;
    [SerializeField] private Transform _playerImg;
    [SerializeField] private SelectEndAfter _endPanel;
    private Transform _canvasTrm;

    [field:SerializeField] public bool IsOnPanel { get; set; }

    private void Awake()
    {
        _canvasTrm = GameObject.Find("Canvas").transform;
    }

    private void Start()
    {
        name = "GameEndPanel";
        _playerImg.DOShakeRotation(0.5f, 100, 10).SetLoops(-1);
    }

    private void Update()
    {
        if(Input.anyKey && !IsOnPanel)
        {
            IsOnPanel = true;
            Instantiate(_endPanel, _canvasTrm);
        }
    }
}
