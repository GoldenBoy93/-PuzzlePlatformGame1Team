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

        // OnClick �̺�Ʈ ���� (��ư �ν����Ϳ��� �����������)
        restartButton.onClick.AddListener(OnClickRestartButton);
        exitButton.onClick.AddListener(OnClickExitButton);
    }

    public void OnClickRestartButton()
    {
        // ���弼���� 0��° ������ ���ư��� (Intro��)
        SceneManager.LoadScene(0);

        // ���������� ���ư��� �� �Ŷ�� �÷��̾ Ư�� ��ҷ� ���ư��� �Լ�
    }

    public void OnClickExitButton()
    {
        // ���ø����̼� ���� (�����Ϳ����� �۵����� ����)
        Application.Quit();
    }    
}