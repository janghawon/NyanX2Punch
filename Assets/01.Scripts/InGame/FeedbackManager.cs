using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Cinemachine;
using System;

public class FeedbackManager : MonoBehaviour
{
    public static FeedbackManager Instance;
    [SerializeField] private CinemachineImpulseSource _source;

    private void Awake()
    {
        Instance = this;
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
