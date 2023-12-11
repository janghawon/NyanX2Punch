using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;

public class RejectPlayerPanel : MonoBehaviour
{
    [SerializeField] private Image _blackPanel;
    [SerializeField] private Transform _rejectPanel;

    private void Start()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(_blackPanel.DOFade(0.4f, 0.2f));
        seq.Join(_rejectPanel.DOLocalMoveY(0, 0.2f));
    }

    public void Accept()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(_blackPanel.DOFade(0f, 0.2f));
        seq.Join(_rejectPanel.DOLocalMoveY(-310, 0.2f));
        seq.AppendCallback(() => Destroy(gameObject));
    }
}
