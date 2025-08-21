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
            transform.SetParent(null); // �θ�(Canvas)���� �и�
            Destroy(gameObject); // �̹� �ٸ� ������ �ı�
            return;
        }
        _instance = this; // ���⼭ ���
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
            // ���� �ð� ���
            yield return new WaitForSecondsRealtime(4f);
            // ���� ������ �Է� Ȱ��ȭ
            _cinematicCam.Priority = 0;
            _freeLookCam.Priority = 10;
            _controller.LockOnInput(0);
        }
    }

    public void LockOnCam(bool canmove)
    {
        if (canmove)
        {
            // �κ��丮 ������ �� �� ī�޶� ����
            _freeLookCam.m_XAxis.m_InputAxisName = "";
            _freeLookCam.m_YAxis.m_InputAxisName = "";
        }
        else
        {
            // �κ��丮 ������ �� �� �ٽ� ������
            _freeLookCam.m_XAxis.m_InputAxisName = "Mouse X";
            _freeLookCam.m_YAxis.m_InputAxisName = "Mouse Y";
        }
    }
}