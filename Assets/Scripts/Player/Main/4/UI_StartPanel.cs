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
              UnityEditor.EditorApplication.isPlaying = false; // 에디터 플레이 모드 종료
        #else
              Application.Quit(); // 빌드된 게임 종료
        #endif
    }
}