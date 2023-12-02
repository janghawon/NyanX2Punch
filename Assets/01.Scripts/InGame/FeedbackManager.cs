using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Unity.Netcode;

public enum FXType
{
    jump,
    impact,
    spark
}

public class FeedbackManager : MonoBehaviour
{
    public static FeedbackManager Instance;
    [SerializeField] private List<GameObject> _effectList = new List<GameObject>();
    [SerializeField] private CinemachineImpulseSource _source;

    private void Awake()
    {
        Instance = this;
    }

    [ServerRpc(RequireOwnership =false)]
    public void MakeFxRpc(FXType type, Vector2 pos)
    {
        Instantiate(_effectList[(int)type], pos, Quaternion.identity);
    }

    [ServerRpc(RequireOwnership = false)]
    public void MakeFxRpc(FXType type, Vector2 pos, Quaternion rot)
    {
        Instantiate(_effectList[(int)type], pos, rot);
    }

    public void ShaekScreen()
    {
        _source.GenerateImpulse();
    }

    public void StopTime(float sec)
    {
        StartCoroutine(StopTimeCo(sec));
    }

    IEnumerator StopTimeCo(float sec)
    {
        Time.timeScale = 0.3f;
        yield return new WaitForSeconds(sec);
        Time.timeScale = 1;
    }
}
