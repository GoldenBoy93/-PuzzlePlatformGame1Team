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
                // �� �ȿ� AudioManager ã��
                _instance = FindObjectOfType<DirectionManager>();

                // ������ ���� ����
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
            Destroy(gameObject); // �̹� �ٸ� ������ �ı�
            return;
        }

        _instance = this; // ���⼭ ���
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
        // ���� �ð� ���
        yield return new WaitForSecondsRealtime(5.5f);
        // ���� ������ �Է� Ȱ��ȭ
        _playerController.LockInputOff();
    }
}