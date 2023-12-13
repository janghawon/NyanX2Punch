using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MapPanel : MonoBehaviour
{
    private Image _bgImg;
    [SerializeField] private Image _panel;
    [SerializeField] private Sprite _sprite;
    [SerializeField] private Color _unSelectColor;
    [SerializeField] private Color _selectColor;
    [SerializeField] private SelectMapPanel _selectMapPanel;
    [SerializeField] private MapType _myMapType;

    private void Awake()
    {
        _bgImg = GameObject.Find("Canvas/BackGround").GetComponent<Image>();
    }

    public void UnSelected()
    {
        _panel.color = _unSelectColor;
    }

    public void SelectThiePanel()
    {
        _selectMapPanel.MapSet(_myMapType);
        _panel.color = _selectColor;
        _bgImg.sprite = _sprite;
    }

    public void EnterPointerThisPanel()
    {
        _selectMapPanel.MarkSet(transform.localPosition);
    }
}
