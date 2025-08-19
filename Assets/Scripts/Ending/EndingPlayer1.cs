using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class EndingPlayer1 : MonoBehaviour
{
    public GameObject player2;
    private EndingPlayer2 player2Script;
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        player2Script = player2.GetComponent<EndingPlayer2>();
    }

    public void PresentChocolate1()
    {
        animator.Play("Player1_Present1");
    }

    public void PresentChocolate2()
    {
        animator.Play("Player1_Present2");
    }

    public void PresentChocolate3()
    {
        animator.Play("Player1_Present3");
    }
}