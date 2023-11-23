using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GameManager : MonoBehaviour
{
    private static GameManager _instance;
    public static GameManager Instanace
    {
        get
        {
            if (_instance != null) return _instance;
            _instance = FindObjectOfType<GameManager>();
            if (_instance == null)
            {
                Debug.LogError("Not Exist GameManager");
            }
            return _instance;
        }
    }

    [SerializeField] private Camera _mainCam;

    public Vector2 WorldToScreenPos(Vector3 worldPos)
    {
        return _mainCam.WorldToScreenPoint(worldPos);
    }

    public Vector2 ScreenToWorldPos(Vector3 screenPos)
    {
        return _mainCam.ScreenToWorldPoint(screenPos);
    }
}
