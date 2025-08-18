using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EventMovingStatue : MonoBehaviour
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponentInParent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        // Ư�� �±׸� ���� ��ü�� �浹���� ���� �۵��ϵ��� ����
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player Enter");

            // �ִϸ��̼� �Ķ���� ���� �ִϸ��̼� Ŭ�� �̸����� �ٷ� ���
            animator.Play("StatueMove");
        }
    }
}
