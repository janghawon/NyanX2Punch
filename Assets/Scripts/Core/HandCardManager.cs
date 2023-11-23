using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class HandCardManager : MonoBehaviour
{
    private static HandCardManager _instance;
    public static HandCardManager Instanace
    {
        get
        {
            if (_instance != null) return _instance;
            _instance = FindObjectOfType<HandCardManager>();
            if (_instance == null)
            {
                Debug.LogError("Not Exist H_CardManager");
            }
            return _instance;
        }
    }
}
