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
    [SerializeField] private CinemachineBlendListCamera _cinematicCam;
    [SerializeField] private CinemachineFreeLook freeLookCam;

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

        _controller.LockInputOn();
    }


    public void Direction()
    {
        StartCoroutine(IntroSequence());
    }

    public IEnumerator IntroSequence()
    {
        if (_cinematicCam != null)
        {
            Cursor.lockState = CursorLockMode.Locked;
            _cinematicCam.Priority = 10;
            // 연출 시간 대기
            yield return new WaitForSecondsRealtime(5.5f);
            // 연출 끝나면 입력 활성화

            _controller.LockInputOff();
        }
    }

    public void CameraMove(bool canmove)
    {
        if (!canmove)
        {
            // 인벤토리 열렸을 때 → 카메라 멈춤
            freeLookCam.m_XAxis.m_InputAxisName = "";
            freeLookCam.m_YAxis.m_InputAxisName = "";
        }
        else
        {
            // 인벤토리 닫혔을 때 → 다시 움직임
            freeLookCam.m_XAxis.m_InputAxisName = "Mouse X";
            freeLookCam.m_YAxis.m_InputAxisName = "Mouse Y";
        }
    }
}