using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public abstract class SpectrumBase : MonoBehaviour
{
    [SerializeField] protected RectTransform[] _visualGroup = new RectTransform[8];
    private AudioSource _audioSur;
    protected float[] _samples = new float[64];
    [SerializeField] protected Vector2 _startSizeDelta;
    protected float _spectrumValue;

    private void Start()
    {
        _audioSur = AudioManager.Instance.bgmSource;
        
        for (int i = 0; i < _visualGroup.Length; i++)
        {
            _visualGroup[i].sizeDelta = _startSizeDelta;
        }
    }

    private void Update()
    {
        if (_audioSur.volume == 0)
        {
            for(int i = 0; i < _visualGroup.Length; i++)
            {
                _visualGroup[i].sizeDelta = Vector2.zero;
            }
        }
        _audioSur.GetSpectrumData(_samples, 0, FFTWindow.Rectangular);
        RhythmVisuallizing();
    }

    protected abstract void RhythmVisuallizing();
}
