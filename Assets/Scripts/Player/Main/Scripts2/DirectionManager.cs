using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DirectionManager : MonoBehaviour
{
    public Animator _animator;
    public PlayerController1 _playerController1;

    public static DirectionManager Instance;
    void Awake()
    {
        if (Instance == null) Instance = this;
        else Destroy(gameObject);
    }
    void Start()
    {
        StartCoroutine(IntroSequence());
    }

    void Update()
    {
    }
    IEnumerator IntroSequence()
    {
        _playerController1.LockInputOn();
        // ���� �ð� ���
        yield return new WaitForSecondsRealtime(5.5f);
        // ���� ������ �Է� Ȱ��ȭ
        _playerController1.LockInputOff();
    }
}
