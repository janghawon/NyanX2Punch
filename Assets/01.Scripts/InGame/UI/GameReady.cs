using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class GameReady : MonoBehaviour
{
    [SerializeField] private Transform _hostPanel;
    [SerializeField] private Transform _playerPanel;

    [SerializeField] private TextMeshProUGUI _hostName;
    [SerializeField] private TextMeshProUGUI _playerName;

    [SerializeField] private TextMeshProUGUI[] _vsText;
}
