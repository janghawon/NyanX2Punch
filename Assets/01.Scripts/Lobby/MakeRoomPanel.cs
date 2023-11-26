using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using DG.Tweening;
using TMPro;

public class MakeRoomPanel : MonoBehaviour
{
    public RoomManagement roomManageMent;
    [SerializeField] private Transform _inputPanel;
    [SerializeField] private Image _blackPanel;
    [SerializeField] private TextMeshProUGUI _syntex;
    [SerializeField] private TMP_InputField _roomNameField;

    private int _nameLimit = 30;
    private string[] _unSuitableWord = 
        { "시발", "개새끼", "병신", "장애", "동진", "정민", "보민", "지호", "좆" ,"섹스", "sex", "애미"};

    private string roomname;

    private void Start()
    {
        ActivePanel();
    }

    public void ActivePanel()
    {
        _blackPanel.enabled = true;
        _inputPanel.DOLocalMoveY(0, 0.3f);
    }
    public void UnActivePanel()
    {
        Sequence seq = DOTween.Sequence();
        seq.Append(_inputPanel.DOLocalMoveY(-728f, 0.3f));
        seq.Join(_blackPanel.DOFade(0, 0.3f));
        seq.AppendCallback(() =>
        {
            Destroy(gameObject);
        });
    }

    public void SaveName(string changeStr)
    {
        roomname = changeStr;
    }

    public void CreateRoom()
    {
        if(CheckCanMakeRoom(roomname))
        {
            Debug.Log("방 파");
            UnActivePanel();
            roomManageMent.HandleCreateLobby(roomname, UnActivePanel, _syntex);
        }
        else
        {
            _syntex.color = Color.red;
        }
    }

    private bool CheckCanMakeRoom(string roomName)
    {
        if (roomName.Length == 0)
        {
            _syntex.text = $"최소 한 글자는 입력하셔야 합니다.";
            
            return false;
        }
        if (roomName.Length > _nameLimit)
        {
            _syntex.text = $"방 이름은 {_nameLimit}보다 길 수 없습니다.";
            return false;
        }
        foreach(string s in _unSuitableWord)
        {
            if (roomName.Contains(s))
            {
                _syntex.text = "부적절한 단어를 포함한 방 이름은 사용할 수 없습니다.";
                return false;
            }
        }
        if (string.IsNullOrEmpty(roomName))
        {
            _syntex.text = "방 이름은 공백일 수 없습니다.";
            return false;
        }
        //통과!
        return true;
    }
}
