using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using Unity.Netcode;

public enum FXType
{
    jump,
    impact,
    spark,
    smoke,
    die_smoke,
    burst
}

public class FeedbackManager : NetworkBehaviour
{
    public static FeedbackManager Instance;
    [SerializeField] private List<GameObject> _effectList = new List<GameObject>();
    [SerializeField] private CinemachineVirtualCamera _vcam;
    [SerializeField] private CinemachineImpulseSource _source;
    [SerializeField] private CinemachineFollowZoom _zoom;

    private Coroutine _timeStopCo;
    private bool _isInTimelerp;

    private void Awake()
    {
        Instance = this;
    }

    [ServerRpc(RequireOwnership = false)]
    public void MakeFxServerRpc(FXType type, Vector2 pos)
    {
        MakeFxClientRpc(type, pos);
    }

    [ClientRpc]
    public void MakeFxClientRpc(FXType type, Vector2 pos)
    {
        Instantiate(_effectList[(int)type], pos, Quaternion.identity);
    }

    [ServerRpc(RequireOwnership = false)]
    public void MakeFxServerRpc(FXType type, Vector2 pos, Quaternion rot)
    {
        MakeFxClientRpc(type, pos, rot);
    }

    [ClientRpc]
    public void MakeFxClientRpc(FXType type, Vector2 pos, Quaternion rot)
    {
        Instantiate(_effectList[(int)type], pos, rot);
    }

    public void ShaekScreen(Vector3 value)
    {
        _source.m_DefaultVelocity = value;
        _source.GenerateImpulse();
    }

    public void StopTime(float sec, float value)
    {
        if(_isInTimelerp)
        {
            StopCoroutine(_timeStopCo);
            Time.timeScale = 1;
        }

        _isInTimelerp = true;
        _timeStopCo = StartCoroutine(StopTimeCo(sec, value));
    }

    IEnumerator StopTimeCo(float sec, float value)
    {
        Time.timeScale = value;
        yield return new WaitForSeconds(sec);
        _isInTimelerp = false;
        Time.timeScale = 1;
    }

    public void ZoomCam(Transform target, float backTime)
    {
        _vcam.Follow = target;
        _zoom.m_MinFOV = 40;
        StartCoroutine(ZoomCamCo(backTime));
    }

    IEnumerator ZoomCamCo(float backTime)
    {
        yield return new WaitForSeconds(backTime);
        _vcam.Follow = null;
        _vcam.transform.position = new Vector3(0, 0, -10);
        _zoom.m_MinFOV = 60;
    }
}
