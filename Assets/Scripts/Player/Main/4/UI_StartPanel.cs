using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class UI_StartPanel : MonoBehaviour
{
    public GameObject startPanel;

    private void Start()
    {
        startPanel.SetActive(true);
    }

    public void OnStart()
    {
        startPanel.SetActive(false);
        DirectionManager.Instance.Direction();
    }
    public void OnGameOver()
    {
        #if UNITY_EDITOR
              UnityEditor.EditorApplication.isPlaying = false; // ������ �÷��� ��� ����
        #else
              Application.Quit(); // ����� ���� ����
        #endif
    }
}