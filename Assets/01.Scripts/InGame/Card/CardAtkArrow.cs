using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CardAtkArrow : MonoBehaviour
{
    private bool _canTargetting;
    private RectTransform _thisTrm;
    private RectTransform _canvasRect;
    private Transform _hostCard;
    public Transform hostCard
    {
        get
        {
            return _hostCard;
        }
        set
        {
            _hostCard = value;
            _canvasRect = (RectTransform)GameObject.Find("UICanvas").transform;
            _canTargetting = true;
            _thisTrm = (RectTransform)transform;
        }
    }

    private void Update()
    {
        if (!_canTargetting || Time.frameCount % 2 == 0) return;


        Vector2 first;
        RectTransformUtility.
            ScreenPointToLocalPointInRectangle(_canvasRect, 
                                               GameManager.Instance.WorldToScreenPos(_hostCard.position), 
                                               null, out first);
        Vector2 second;
        RectTransformUtility.
            ScreenPointToLocalPointInRectangle(_canvasRect,
                            Input.mousePosition, null, out second);

        _thisTrm.sizeDelta = new Vector2(220,
                              Mathf.Sqrt(Mathf.Pow(second.x - first.x, 2)
                                        +Mathf.Pow(second.y - first.y, 2)));

        _thisTrm.localPosition = (first + second) / 2;

        Vector2 dir = (first - second).normalized;
        float angle = Mathf.Atan2(dir.y, dir.x) * Mathf.Rad2Deg;
        _thisTrm.localRotation = Quaternion.AngleAxis(angle - 90, Vector3.forward);
    }
}
