using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;
using DG.Tweening;
using System.Text.RegularExpressions;

public class ModifyPlayerNamePanel : MonoBehaviour
{
    public RoomManagement roomManageMent;
    public const string nameKey = "userName";
    [SerializeField] private Transform _inputPanel;
    [SerializeField] private Image _blackPanel;
    [SerializeField] private TextMeshProUGUI _syntex;
    [SerializeField] private TMP_InputField _playerNameField;

    private int _nameLimit = 8;
    private string[] _unSuitableWord =
        { "�ù�", "������", "����", "���", "����", "����", "����", "��ȣ", "��" ,"����", "sex", "�ֹ�"};

    private string playername;
    private Regex _checkSpecialWordRegix;

    private void Start()
    {
        _checkSpecialWordRegix = new Regex(@"[^\w\s]");
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
        playername = changeStr;
    }

    public void ModifyPlayerName()
    {
        if (CheckCanModifyName(playername))
        {
            PlayerPrefs.SetString(nameKey, playername);
            roomManageMent.RefreshPlayerName();
            UnActivePanel();
        }
        else
        {
            _syntex.color = Color.red;
        }
    }

    private bool CheckCanModifyName(string playerName)
    {
        if (playerName.Length == 0)
        {
            _syntex.text = $"�ּ� �� ���ڴ� �Է��ؾ� �մϴ�.";

            return false;
        }
        if (playerName.Length > _nameLimit)
        {
            _syntex.text = $"�г����� {_nameLimit}���� �� �� �����ϴ�.";
            return false;
        }
        foreach (string s in _unSuitableWord)
        {
            if (playerName.Contains(s))
            {
                _syntex.text = "�������� �ܾ ������ �г����� ����� �� �����ϴ�.";
                return false;
            }
        }
        if (playerName.Contains(" "))
        {
            _syntex.text = "�г��ӿ� ������ ��� �� �� �����ϴ�.";
            return false;
        }
        if(_checkSpecialWordRegix.IsMatch(playerName))
        {
            _syntex.text = "�г��ӿ� Ư�����ڸ� ��� �� �� �����ϴ�.";
            return false;
        }
        
        //���!
        return true;
    }
}
