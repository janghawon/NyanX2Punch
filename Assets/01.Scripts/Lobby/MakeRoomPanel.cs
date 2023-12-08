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
        { "�ù�", "������", "����", "���", "����", "����", "����", "��ȣ", "��" ,"����", "sex", "�ֹ�"};

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
            _syntex.text = $"�ּ� �� ���ڴ� �Է��ϼž� �մϴ�.";
            
            return false;
        }
        if (roomName.Length > _nameLimit)
        {
            _syntex.text = $"�� �̸��� {_nameLimit}���� �� �� �����ϴ�.";
            return false;
        }
        foreach(string s in _unSuitableWord)
        {
            if (roomName.Contains(s))
            {
                _syntex.text = "�������� �ܾ ������ �� �̸��� ����� �� �����ϴ�.";
                return false;
            }
        }
        if (string.IsNullOrEmpty(roomName))
        {
            _syntex.text = "�� �̸��� ������ �� �����ϴ�.";
            return false;
        }
        //���!
        return true;
    }
}
