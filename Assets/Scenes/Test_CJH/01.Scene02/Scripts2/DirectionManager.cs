using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class DirectionManager : MonoBehaviour
{
    public Animator _animator;

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
        _animator.SetBool("CanMove",false);
        // ���� �ð� ���
        yield return new WaitForSecondsRealtime(5.5f);
        // ���� ������ �Է� Ȱ��ȭ
        _animator.SetBool("CanMove",true);
    }
}
