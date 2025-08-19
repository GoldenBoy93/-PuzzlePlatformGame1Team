using System.Collections;
using System.Collections.Generic;
using UnityEditor.Experimental.GraphView;
using UnityEngine;

public class EventDropBook : MonoBehaviour
{
    private Animator animator;

    private bool hasTriggered = false;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player") && !hasTriggered)
        {
            animator.SetTrigger("EnterCollider");

            hasTriggered = true;

            DirectionManager.Instance.Direction();

        }
    }
}
