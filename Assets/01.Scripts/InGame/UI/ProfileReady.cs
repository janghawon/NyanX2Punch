using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProfileReady : MonoBehaviour
{
    [SerializeField] private GameObject _hostSunglass;
    [SerializeField] private GameObject _clientSunglass;

    public void SetStateReady(GameRole role, bool state)
    {
        GameObject sunglass = role == GameRole.Host ? _hostSunglass : _clientSunglass;
        sunglass.SetActive(state);
    }
}
