using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;

    IntroUI introUI;
    GameUI gameUI;
    GameOverUI gameOverUI;
    private BaseUI currentUI; // 현재 활성화된 UI를 저장하는 변수

    [SerializeField] private Transform uiRootPosition; // UIManager의 자식인 Canvas의 Transform을 저장

    public static UIManager Instance
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        // Awake가 호출 될 때라면 이미 매니저 오브젝트는 생성되어 있는 것이고, '_instance'에 자신을 할당
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // 이미 오브젝트가 존재하는 경우 '자신'을 파괴해서 중복방지
            if (_instance != this)
            {
                Destroy(gameObject);
            }
        }

        GameManager.OnGameStateChanged += OnGameStateChanged; // 이벤트에 자신의 함수내용을 추가
    }

    // GameManager의 이벤트에 넣을 함수
    public void OnGameStateChanged(GameState newState)
    {
        // GameManager의 상태에 따라 UI를 변경
        switch (newState)
        {
            case GameState.Intro:
                DestroyCurrentUI(); // 이전 UI는 파괴
                currentUI = ResourceManager.Instance.InstantiateUI<IntroUI>("IntroUI", uiRootPosition);
                break;

            case GameState.Playing:

                DestroyCurrentUI(); // 이전 UI는 파괴

                currentUI = ResourceManager.Instance.InstantiateUI<GameUI>("GameUI", uiRootPosition);

                // 씬에서 플레이어를 찾아
                PlayerCondition playerCondition = FindObjectOfType<PlayerCondition>();

                if (playerCondition != null)
                {
                    // gameUI의 자식인 UICondition 스크립트를 찾음
                    UICondition uiConditions = currentUI.GetComponentInChildren<UICondition>();

                    if (uiConditions != null)
                    {
                        // 플레이어에게 체력바 스크립트를 연결해줌
                        playerCondition.SetUICondition(uiConditions);
                    }
                }

                break;

            case GameState.Paused:
                currentUI = ResourceManager.Instance.InstantiateUI<PausedUI>("PausedUI", uiRootPosition);
                break;

            case GameState.GameOver:
                DestroyCurrentUI(); // 이전 UI는 파괴
                currentUI = ResourceManager.Instance.InstantiateUI<GameOverUI>("GameOverUI", uiRootPosition);
                break;
        }
    }

    void DestroyCurrentUI()
    {
        // 기존 UI 파괴
        if (currentUI != null)
        {
            Destroy(currentUI.gameObject);
        }
    }

    // 오브젝트가 파괴될 때 이벤트 구독 해제
    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= OnGameStateChanged;
    }
}