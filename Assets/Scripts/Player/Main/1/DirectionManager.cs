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
        //StartCoroutine(SetCineCam());
    }

    IEnumerator SetCineCam()
    {
        yield return null; // �� ������ ��ٸ�
        _cinematicCam = GameObject.Find("Blend List Camera")?.GetComponent<CinemachineVirtualCamera>();
    }

    public IEnumerator IntroSequence()
    {
        if (_cinematicCam != null)
        {
            _controller.LockInputOn();
            _cinematicCam.Priority = 0;

            // ���� �ð� ���
            yield return new WaitForSecondsRealtime(5.5f);
            // ���� ������ �Է� Ȱ��ȭ

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