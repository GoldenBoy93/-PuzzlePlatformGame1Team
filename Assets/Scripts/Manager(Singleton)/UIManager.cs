using System;
using UnityEngine;
using UnityEngine.SceneManagement;

public class UIManager : MonoBehaviour
{
    private static UIManager _instance;

    IntroUI introUI;
    GameUI gameUI;
    GameOverUI gameOverUI;
    private BaseUI currentUI; // ���� Ȱ��ȭ�� UI�� �����ϴ� ����

    [SerializeField] private Transform uiRootPosition; // UIManager�� �ڽ��� Canvas�� Transform�� ����

    public static UIManager Instance
    {
        get
        {
            return _instance;
        }
    }

    private void Awake()
    {
        // Awake�� ȣ�� �� ����� �̹� �Ŵ��� ������Ʈ�� �����Ǿ� �ִ� ���̰�, '_instance'�� �ڽ��� �Ҵ�
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // �̹� ������Ʈ�� �����ϴ� ��� '�ڽ�'�� �ı��ؼ� �ߺ�����
            if (_instance != this)
            {
                Destroy(gameObject);
            }
        }

        GameManager.OnGameStateChanged += OnGameStateChanged; // �̺�Ʈ�� �ڽ��� �Լ������� �߰�
    }

    // GameManager�� �̺�Ʈ�� ���� �Լ�
    public void OnGameStateChanged(GameState newState)
    {
        // GameManager�� ���¿� ���� UI�� ����
        switch (newState)
        {
            case GameState.Intro:
                DestroyCurrentUI(); // ���� UI�� �ı�
                currentUI = ResourceManager.Instance.InstantiateUI<IntroUI>("IntroUI", uiRootPosition);
                break;

            case GameState.Playing:

                DestroyCurrentUI(); // ���� UI�� �ı�

                currentUI = ResourceManager.Instance.InstantiateUI<GameUI>("GameUI", uiRootPosition);

                // ������ �÷��̾ ã��
                PlayerCondition playerCondition = FindObjectOfType<PlayerCondition>();

                if (playerCondition != null)
                {
                    // gameUI�� �ڽ��� UICondition ��ũ��Ʈ�� ã��
                    UICondition uiConditions = currentUI.GetComponentInChildren<UICondition>();

                    if (uiConditions != null)
                    {
                        // �÷��̾�� ü�¹� ��ũ��Ʈ�� ��������
                        playerCondition.SetUICondition(uiConditions);
                    }
                }

                break;

            case GameState.Paused:
                currentUI = ResourceManager.Instance.InstantiateUI<PausedUI>("PausedUI", uiRootPosition);
                break;

            case GameState.GameOver:
                DestroyCurrentUI(); // ���� UI�� �ı�
                currentUI = ResourceManager.Instance.InstantiateUI<GameOverUI>("GameOverUI", uiRootPosition);
                break;
        }
    }

    void DestroyCurrentUI()
    {
        // ���� UI �ı�
        if (currentUI != null)
        {
            Destroy(currentUI.gameObject);
        }
    }

    // ������Ʈ�� �ı��� �� �̺�Ʈ ���� ����
    private void OnDestroy()
    {
        GameManager.OnGameStateChanged -= OnGameStateChanged;
    }
}