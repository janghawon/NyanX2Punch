using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SelectMap : MonoBehaviour
{
    [SerializeField] private SelectMapPanel _panel;
    [SerializeField] private Transform _canvasTrm;

    public void SpawnPanel()
    {
        Instantiate(_panel, _canvasTrm);
    }
}
