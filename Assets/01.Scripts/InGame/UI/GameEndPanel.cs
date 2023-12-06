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

    private void Start()
    {
        _playerImg.DOShakeRotation(0.5f, 100, 10).SetLoops(-1);
    }
}
