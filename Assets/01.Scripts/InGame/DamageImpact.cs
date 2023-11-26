using UnityEngine;
using TMPro;
using DG.Tweening;

public class DamageImpact : MonoBehaviour
{
    [SerializeField] private TextMeshPro _damageTxt;
    [SerializeField] private SpriteRenderer _sr;

    public void SetDamage(int damage)
    {
        _damageTxt.text = damage.ToString();
    }

    private void Start()
    {
        transform.localScale = Vector3.zero;
        Sequence seq = DOTween.Sequence();
        seq.Append(transform.DOScale(Vector3.one * 0.2f, 0.3f));
        seq.AppendInterval(0.2f);
        seq.Append(_sr.DOFade(0, 0.4f));
        seq.Join(_damageTxt.DOFade(0, 0.4f));
        seq.AppendCallback(() =>
        {
            Destroy(gameObject);
        });
    }
}
