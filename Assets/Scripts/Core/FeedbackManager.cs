using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;

public class FeedbackManager : MonoBehaviour
{
    private static FeedbackManager _instance;
    public static FeedbackManager Instanace
    {
        get
        {
            if (_instance != null) return _instance;
            _instance = FindObjectOfType<FeedbackManager>();
            if (_instance == null)
            {
                Debug.LogError("Not Exist FeedbackManager");
            }
            return _instance;
        }
    }

    private CinemachineImpulseSource _source;

    [SerializeField] private List<VFXBase> _vfxList = new List<VFXBase>();

    private void Awake()
    {
        _source = GetComponent<CinemachineImpulseSource>();
    }

    public void ShakeScreen()
    {
        _source.GenerateImpulse();
    }

    public void MakeVFX(VFXType vType, Vector2 spawnPos)
    {
        Instantiate(_vfxList[(int)vType], spawnPos, Quaternion.identity);
    }
}
