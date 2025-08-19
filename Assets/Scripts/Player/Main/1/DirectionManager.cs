using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DirectionManager : MonoBehaviour
{
    private static DirectionManager _instance;
    public static DirectionManager Instance
    {
        get
        {
            if (_instance == null)
            {
                // 씬 안에 AudioManager 찾기
                _instance = FindObjectOfType<DirectionManager>();

                // 없으면 새로 생성
                if (_instance == null)
                {
                    GameObject go = new GameObject("DirectionManager");
                    _instance = go.AddComponent<DirectionManager>();
                }
            }
            return _instance;
        }
    }

    public Animator _animator;
    public PlayerController _playerController;

    void Awake()
    {
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject); // 이미 다른 있으면 파괴
            return;
        }

        _instance = this; // 여기서 등록
        DontDestroyOnLoad(gameObject);
    }



    void Start()
    {
        _animator = GameManager.Instance.PlayerManager.GetComponent<Animator>();
        _playerController = GameManager.Instance.PlayerManager.GetComponent<PlayerController>();
    
        StartCoroutine(IntroSequence());
    }
   
    IEnumerator IntroSequence()
    {
        _playerController.LockInputOn();
        // 연출 시간 대기
        yield return new WaitForSecondsRealtime(5.5f);
        // 연출 끝나면 입력 활성화
        _playerController.LockInputOff();
    }
}