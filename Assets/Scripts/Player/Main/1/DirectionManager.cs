using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;
using static UnityEngine.Rendering.DebugUI;

public class DirectionManager : MonoBehaviour
{
    private static DirectionManager _instance;
    public static DirectionManager Instance
    {
        get
        {
            if (_instance == null)
            {
                _instance = FindObjectOfType<DirectionManager>();

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
    public PlayerController _controller;
    public CinemachineVirtualCamera _cinematicCam;

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
        _animator = SafeFetchHelper.GetOrError<Animator>(Player.Instance.gameObject);
        _controller = SafeFetchHelper.GetOrError<PlayerController>(Player.Instance.gameObject);
        //StartCoroutine(SetCineCam());
    }

    IEnumerator SetCineCam()
    {
        yield return null; // 한 프레임 기다림
        _cinematicCam = GameObject.Find("Blend List Camera")?.GetComponent<CinemachineVirtualCamera>();
    }

    public IEnumerator IntroSequence()
    {
        if (_cinematicCam != null)
        {
            _controller.LockInputOn();
            _cinematicCam.Priority = 0;

            // 연출 시간 대기
            yield return new WaitForSecondsRealtime(5.5f);
            // 연출 끝나면 입력 활성화

            _controller.LockInputOff();
            _cinematicCam.Priority = 10;
        }
    }

    public void Direction()
    {
        Cursor.lockState = CursorLockMode.Locked;

        StartCoroutine(IntroSequence());
    }
}