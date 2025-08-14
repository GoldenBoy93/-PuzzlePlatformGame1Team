using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PausedUI : BaseUI
{
    [SerializeField] private Button returnButton;

    public override void Init(UIManager uiManager)
    {
        base.Init(uiManager);

        // OnClick �̺�Ʈ ���� (��ư �ν����Ϳ��� �����������)
        returnButton.onClick.AddListener(OnClickReturnButton);
    }

    public void OnClickReturnButton()
    {
        // GameManager�� ReturnGame �޼��� ȣ��
        GameManager.Instance.ReturnGame();
    }
}