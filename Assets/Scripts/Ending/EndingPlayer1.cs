using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static UnityEditor.Experimental.GraphView.GraphView;

public class EndingPlayer1 : MonoBehaviour
{
    public GameObject player2;
    private EndingPlayer2 player2Script;
    Animator animator;
    private Rigidbody rb;

    public Vector3 targetPosition;

    public GameObject chocolate;

    private void Start()
    {
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody>();
        player2Script = player2.GetComponent<EndingPlayer2>();
    }

    public void Present()
    {
        animator.Play("Player1_Present");
        StartCoroutine(Chocolate());
    }

    public void Shame()
    {
        animator.Play("Player1_Shame");
    }

    public void Idle()
    {
        animator.Play("Player1_Idle");
    }

    public void Walk()
    {
        animator.Play("Player1_Walk");
    }

    public void Hug()
    {
        SetTargetTransform(new Vector3(-3,-7,6));
        animator.Play("Player1_Hug");
    }

    public void HugEnd()
    {
        player2Script.Surprise();
    }

    public void Backwalk()
    {
        animator.Play("Player1_Backwalk");
    }

    public void BackwalkEnd()
    {
        player2Script.Idle3();
    }

    public void Okay()
    {
        animator.Play("Player1_Okay");
    }

    public void Turn()
    {
        animator.Play("Player1_Turn");
    }

    public void Victory()
    {
        animator.Play("Player1_Victory");
    }
    void OnAnimatorMove()
    {
        // 1. �ִϸ��̼��� �����Ϸ��� ��ġ ��ȭ���� �����ɴϴ�.
        // �� ���� �ִϸ��̼� Ŭ�� ��ü�� �̵���(Root Motion)�Դϴ�.
        Vector3 deltaPosition = animator.deltaPosition;

        // 2. �� ��ġ ��ȭ���� Rigidbody�� ���� ĳ������ ���� ��ġ�� �����մϴ�.
        // �̷��� �ϸ� ���� �ִϸ��̼��� ������ ��ġ���� ���� �ִϸ��̼��� �̾ ����˴ϴ�.
        if (rb != null)
        {
            rb.MovePosition(rb.position + deltaPosition);
        }
    }

    // �ٸ� ��ũ��Ʈ���� �� �Լ��� ȣ���Ͽ� ��ǥ ��ġ�� ȸ���� ������ �� �ֽ��ϴ�.
    public void SetTargetTransform(Vector3 newPosition)
    {
        targetPosition = newPosition;
    }

    IEnumerator Chocolate()
    {
        yield return new WaitForSeconds(1.2f);
        chocolate.SetActive(true);
        yield return new WaitForSeconds(1.5f);
        chocolate.SetActive(false);
    }
}