using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.SceneManagement;

public class TitleScene : MonoBehaviour
{
    private bool _isLoadingComplete;
    [SerializeField] private TextMeshProUGUI _loadingText;

    public void CompleteReady()
    {
        _isLoadingComplete = true;
        _loadingText.text = "Press AnyKey To Start!!";
        _loadingText.transform.localPosition = new Vector3(0, _loadingText.transform.localPosition.y);
    }

    private void Update()
    {
        if (!_isLoadingComplete) return;
        if (Input.anyKeyDown)
        {
            SceneManager.LoadScene(SceneList.MenuScene);
            _isLoadingComplete = false;
        }
    }
}
