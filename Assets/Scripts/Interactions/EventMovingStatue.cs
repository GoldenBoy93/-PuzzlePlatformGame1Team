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
        // 특정 태그를 가진 객체가 충돌했을 때만 작동하도록 설정
        if (other.CompareTag("Player"))
        {
            Debug.Log("Player Enter");

            // 애니메이션 파라미터 없이 애니메이션 클립 이름으로 바로 재생
            animator.Play("StatueMove");
        }
    }
}
