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

        // OnClick 이벤트 생성 (버튼 인스펙터에서 설정해줘야함)
        returnButton.onClick.AddListener(OnClickReturnButton);
    }

    public void OnClickReturnButton()
    {
        // GameManager의 ReturnGame 메서드 호출
        GameManager.Instance.ReturnGame();
    }
}