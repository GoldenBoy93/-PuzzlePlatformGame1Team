using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingPlayer2 : MonoBehaviour
{
    public GameObject player1;
    private EndingPlayer1 player1Script;
    Animator animator;

    private void Start()
    {
        animator = GetComponent<Animator>();
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
}
