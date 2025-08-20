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
            Destroy(gameObject); // �̹� �ٸ� ������ �ı�
            return;
        }
        _instance = this; // ���⼭ ���
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
            // ���� �ð� ���
            yield return new WaitForSecondsRealtime(5.5f);
            // ���� ������ �Է� Ȱ��ȭ

            _controller.LockInputOff();
        }
    }

    public void CameraMove(bool canmove)
    {
        if (!canmove)
        {
            // �κ��丮 ������ �� �� ī�޶� ����
            freeLookCam.m_XAxis.m_InputAxisName = "";
            freeLookCam.m_YAxis.m_InputAxisName = "";
        }
        else
        {
            // �κ��丮 ������ �� �� �ٽ� ������
            freeLookCam.m_XAxis.m_InputAxisName = "Mouse X";
            freeLookCam.m_YAxis.m_InputAxisName = "Mouse Y";
        }
    }
}