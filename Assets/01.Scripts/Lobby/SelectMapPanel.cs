using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class SelectMapPanel : MonoBehaviour
{
    [SerializeField] private Image _blackPanel;
    [SerializeField] private Transform _inputPanel;
    [SerializeField] private Transform _selectMark;

    [SerializeField] private MapPanel[] _mapPanelArr;

    private MapType _selectMapType;

    private void Start()
    {
        ActivePanel();
    }

    public void ActivePanel()
    {
        _blackPanel.enabled = true;
        _inputPanel.DOLocalMoveY(0, 0.3f);
    }
    public void UnActivePanel()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(_inputPanel.DOLocalMoveY(-728f, 0.3f));
        seq.Join(_blackPanel.DOFade(0, 0.3f));
        seq.AppendCallback(() =>
        {
            Destroy(gameObject);
        });
    }

    public void Cancle()
    {
        UnActivePanel();
    }

    public void Accept()
    {
        HostSingleton.Instnace.GamaManager.selectMapType = _selectMapType;
        UnActivePanel();
    }

    public void MapSet(MapType mt)
    {
        foreach(MapPanel mp in _mapPanelArr)
        {
            mp.UnSelected();
        }
        _selectMapType = mt;
    }

    public void MarkSet(Vector2 pos)
    {
        _selectMark.localPosition = pos;
    }
}
