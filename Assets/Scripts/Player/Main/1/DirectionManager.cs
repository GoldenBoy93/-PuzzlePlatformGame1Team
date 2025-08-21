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
        if (_instance != null && _instance != this)
        {
            Destroy(gameObject); // �̹� �ٸ� ������ �ı�
            return;
        }
        _instance = this; // ���⼭ ���
        DontDestroyOnLoad(gameObject);
    }


    Animator _animator;
    PlayerController _controller;
    [Header("IntroCamera")]
    [SerializeField] private CinemachineBlendListCamera _cinematicCam;
    [SerializeField] private CinemachineFreeLook _freeLookCam;

    [Header("InGameCamera")]
    private int eq;

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
            // ���� �ð� ���
            yield return new WaitForSecondsRealtime(4f);
            // ���� ������ �Է� Ȱ��ȭ

            _controller.LockInputOff();
        }
    }

    public void LockCamOn(bool canmove)
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