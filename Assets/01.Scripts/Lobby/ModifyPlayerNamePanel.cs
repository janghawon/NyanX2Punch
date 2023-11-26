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
        { "시발", "개새끼", "병신", "장애", "동진", "정민", "보민", "지호", "좆" ,"섹스", "sex", "애미"};

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
            Debug.Log("닉네임 설정");
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
            _syntex.text = $"최소 한 글자는 입력해야 합니다.";

            return false;
        }
        if (playerName.Length > _nameLimit)
        {
            _syntex.text = $"닉네임은 {_nameLimit}보다 길 수 없습니다.";
            return false;
        }
        foreach (string s in _unSuitableWord)
        {
            if (playerName.Contains(s))
            {
                _syntex.text = "부적절한 단어를 포함한 닉네임은 사용할 수 없습니다.";
                return false;
            }
        }
        if (playerName.Contains(" "))
        {
            _syntex.text = "닉네임엔 공백을 사용 할 수 없습니다.";
            return false;
        }
        if(_checkSpecialWordRegix.IsMatch(playerName))
        {
            _syntex.text = "닉네임엔 특수문자를 사용 할 수 없습니다.";
            return false;
        }
        
        //통과!
        return true;
    }
}
