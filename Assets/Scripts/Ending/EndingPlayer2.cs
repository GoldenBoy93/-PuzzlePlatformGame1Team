using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class EndingPlayer2 : MonoBehaviour
{
    public GameObject player1;
    private EndingPlayer1 player1Script;
    Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
        player1Script = player1.GetComponent<EndingPlayer1>();
    }

    public void IdleEnd()
    {
        player1Script.PresentChocolate1();
    }
}
