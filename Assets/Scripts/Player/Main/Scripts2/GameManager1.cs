using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class GameManager1 : MonoBehaviour //게임초기화, 레벨관리
{
    public static GameManager1 Instance;
    int stageLevel = 1;

    private void Awake()
    {
        if (Instance == null)
        {
            Instance = this;
            DontDestroyOnLoad(gameObject);
            stageLevel = 1;
        }
        else
        {
            Destroy(gameObject);
        }
    }

    private void OnEnable()
    {
        SceneManager.sceneLoaded += OnSceneLoaded;
    }

    private void OnDisable()
    {
        SceneManager.sceneLoaded -= OnSceneLoaded;
    }

    private void OnSceneLoaded(Scene scene, LoadSceneMode mode)
    {
        //if (scene.buildIndex == (int)Escene.NormalStage)
        //{
        //    FindObjectOfType<BoardManager>()?.SetupScene(stageLevel);
        //
        //    //플레이어 생성 위치 지정
        //    if (Player.Instance != null)
        //        Player.Instance.transform.position = new Vector3(7.5f, 7.5f, 0f);
        //
        //    EnemySpawnManager spawnManager = FindObjectOfType<EnemySpawnManager>();
        //    if (spawnManager != null)
        //        spawnManager.StartSpawning(stageLevel);
        //
        //}
        //else if (scene.buildIndex == (int)Escene.BossStage)
        //{
        //    // 보스 씬 초기화도 여기서 가능
        //
        //    //보스씬에서도 위치 지정
        //    if (Player.Instance != null)
        //        Player.Instance.transform.position = new Vector3(3.5f, 2f, 0f);
        //}
    }

    public void StageCleared()
    {
        Debug.Log("Stage Level increased to: " + stageLevel);
        stageLevel++;
    }
    public void GameOver()
    {
        Destroy(gameObject);
    }

    public void TakeDamage(int amount)
    {
        //if (Player.Instance != null)
        //{
        //    Player.Instance.TakeDamage(amount);
        //    Debug.Log($"Player took damage: -{amount}, currentHP: {Player.Instance.currentHP}");
        //    if (Player.Instance.currentHP <= 0)
        //        GameOver();
        //}
    }

    public void IncreaseHP(int amount = 1)
    {
        //if (Player.Instance != null)
        //{
        //    Player.Instance.currentHP = Mathf.Min(Player.Instance.currentHP + amount, Player.Instance.maxHP);
        //    Player.Instance.RefreshStatsUI();
        //    Debug.Log($"Player healed: + {amount}, currentHP: {Player.Instance.currentHP}");
        //}
    }
}