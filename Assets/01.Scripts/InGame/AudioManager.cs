using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public enum BGMType
{
    title,
    lobby,
    battle,
    victory
}

public enum SFXType
{
    btnsound,
    hit,
    jump,
    swipe
}

public class AudioManager : MonoBehaviour
{
    public static AudioManager Instance;

    [SerializeField] private List<AudioClip> _sfxClipList = new List<AudioClip>();
    [SerializeField] private List<AudioClip> _bgmClipList = new List<AudioClip>();

    [SerializeField] private AudioSource _sfxSource;
    [SerializeField] private AudioSource _bgmSource;

    private bool _isBgmPlaying;


    private void Awake()
    {
        Instance = this;
        DontDestroyOnLoad(this.gameObject);
    }

    public void PlayBGM(BGMType type)
    {
        if(_isBgmPlaying)
        {
            _bgmSource.Stop();
        }
        _isBgmPlaying = true;

        _bgmSource.clip = _bgmClipList[(int)type];
        _bgmSource.Play();
    }

    public void PlaySFX(SFXType type)
    {
        AudioSource source = new AudioSource();
        source.clip = _sfxClipList[(int)type];
        source.Play();
    }
}
