using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;
using UnityEngine.UI;
using static UnityEngine.EventSystems.EventTrigger;

public class GameOverUI : BaseUI
{
    [SerializeField] private Button restartButton;
    [SerializeField] private Button exitButton;

    public override void Init(UIManager uiManager)
    {
        base.Init(uiManager);

        // OnClick 이벤트 생성 (버튼 인스펙터에서 설정해줘야함)
        restartButton.onClick.AddListener(OnClickRestartButton);
        exitButton.onClick.AddListener(OnClickExitButton);
    }

    public void OnClickRestartButton()
    {
        // 빌드세팅의 0번째 씬으로 돌아가기 (Intro씬)
        SceneManager.LoadScene(0);

        // 게임중으로 돌아가게 할 거라면 플레이어가 특정 장소로 돌아가는 함수
    }

    public void OnClickExitButton()
    {
        // 애플리케이션 종료 (에디터에서는 작동하지 않음)
        Application.Quit();
    }    
}