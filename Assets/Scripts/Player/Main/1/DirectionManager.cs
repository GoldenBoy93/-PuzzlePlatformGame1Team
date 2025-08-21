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
    void Awake()
    {
        Debug.Log("UI_Manager Awake in scene: " + gameObject.scene.name);
        if (_instance != null && _instance != this)
        {
            Debug.Log("Duplicate UIManager found, destroying this one: " + gameObject.name);
            transform.SetParent(null); // 부모(Canvas)에서 분리
            Destroy(gameObject); // 이미 다른 있으면 파괴
            return;
        }
        _instance = this; // 여기서 등록
        DontDestroyOnLoad(gameObject);
    }


    Animator _animator;
    PlayerController _controller;
    [Header("IntroCamera")]
    [SerializeField] private CinemachineVirtualCamera _introCam;
    [SerializeField] private CinemachineBlendListCamera _cinematicCam;
    [SerializeField] private CinemachineFreeLook _freeLookCam;

    [Header("InGameCamera")]
    private int eq;

    void Start()
    {
        _animator = SafeFetchHelper.GetOrError<Animator>(Player.Instance.gameObject);
        _controller = SafeFetchHelper.GetOrError<PlayerController>(Player.Instance.gameObject);
        _controller.LockOnInput(1);
    }


    public void Direction_Intro()
    {
        StartCoroutine(IntroSequence());
        Destroy(_introCam);
    }

    public void OnDirection(bool start)
    {
        if (start)
        {
            _freeLookCam.Priority = 0;
        }
        else
        {
            _freeLookCam.Priority = 10;
        }
    }

    public IEnumerator IntroSequence()
    {
        if (_cinematicCam != null)
        {
            _controller.LockOnInput(1);
            Cursor.lockState = CursorLockMode.Locked;
            _cinematicCam.Priority = 10;
            // 연출 시간 대기
            yield return new WaitForSecondsRealtime(4f);
            // 연출 끝나면 입력 활성화
            _cinematicCam.Priority = 0;
            _freeLookCam.Priority = 10;
            _controller.LockOnInput(0);
        }
    }

    public void LockOnCam(bool canmove)
    {
        if (canmove)
        {
            // 인벤토리 열렸을 때 → 카메라 멈춤
            _freeLookCam.m_XAxis.m_InputAxisName = "";
            _freeLookCam.m_YAxis.m_InputAxisName = "";
        }
        else
        {
            // 인벤토리 닫혔을 때 → 다시 움직임
            _freeLookCam.m_XAxis.m_InputAxisName = "Mouse X";
            _freeLookCam.m_YAxis.m_InputAxisName = "Mouse Y";
        }
    }
}