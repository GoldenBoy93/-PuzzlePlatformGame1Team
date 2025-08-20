using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingPlayer2 : MonoBehaviour
{
    public GameObject player1;
    private EndingPlayer1 player1Script;
    Animator animator;
    private Rigidbody rb;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        player1Script = player1.GetComponent<EndingPlayer1>();
    }

    public void IdleEnd()
    {
        player1Script.Present();
    }

    public void Surprise()
    {
        animator.Play("Player2_Surprise");
    }

    public void Idle3()
    {
        animator.Play("Player2_Idle3");
    }

    public void Idle2()
    {
        animator.Play("Player2_Idle2");
    }

    public void FollowHand()
    {
        animator.Play("Player2_FollowHand");
    }

    public void FollowHandEnd()
    {
        player1Script.Okay();
    }

    public void Walk()
    {
        animator.Play("Player2_Walk");
    }

    void OnAnimatorMove()
    {
        // 애니메이션 위치 변화량을 가져옴
        Vector3 deltaPosition = animator.deltaPosition;

        // 위치 변화량을 캐릭터의 실제 위치에 적용
        if (rb != null)
        {
            rb.MovePosition(rb.position + deltaPosition);
        }
    }
}
