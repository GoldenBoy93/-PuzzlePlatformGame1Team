using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController2 : MonoBehaviour
{
    [Header("Movement")]
    private Vector2 MovementInput; // �Է°� ���� Vector3 ���� ����
    public float moveSpeed = 1f;
    public float jumpPower = 1f;

    [HideInInspector]
    Rigidbody rb;
    Animator anim;
    Transform cameraTransform;

    void Start()
    {
        // GameManager�� PlayerInstance�� ���� playerRigidbody�� �����Ͽ� ������ ����
        rb = GameManager.Instance.PlayerInstance.playerRigidbody;
        anim = GameManager.Instance.PlayerInstance.playerAnimator;
    }

    void FixedUpdate()
    {
        // ���� ī�޶��� transform ����
        cameraTransform = Camera.main.transform;

        // FixedUpdate���� Move() �Լ��� ȣ���Ͽ� DeltaTime�� ���� �� �̵�
        Move();
    }

    // InputSystem���� ȣ�� �޴� �Լ�
    public void OnMoveInput(InputAction.CallbackContext context)
    {
        // context�� phase(����)�� Performed(�Է���) ������ ��
        if (context.phase == InputActionPhase.Performed)
        {
            // context�� Vector2 ���� �Ҵ�
            MovementInput = context.ReadValue<Vector2>();
        }

        // context�� phase(����)�� Performed(�Է����) ������ ��
        else if (context.phase == InputActionPhase.Canceled)
        {
            // Vector2(0,0)�� �Ҵ�
            MovementInput = Vector2.zero;
        }
    }

    // InputSystem���� ȣ�� �޴� �Լ�
    public void OnJumpInput(InputAction.CallbackContext context)
    {
        // context�� phase(����)�� Performed(�Է���) ������ ��
        if (context.phase == InputActionPhase.Started)
        {
            // context�� Vector2 ���� �Ҵ�
            rb.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
            
            anim.SetTrigger("Jumping");
        }
    }

    void Move()
    {
        // ī�޶��� �� ����� ������ ���� ���͸� ������
        // y���� 0���� ����� ���� ���� ���͸� ���
        Vector3 cameraForward = Vector3.Scale(cameraTransform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 cameraRight = Vector3.Scale(cameraTransform.right, new Vector3(1, 0, 1)).normalized;

        // ī�޶� ���� �������� �̵� ���� ���
        Vector3 moveDirection = (cameraForward * MovementInput.y + cameraRight * MovementInput.x).normalized;

        // ĳ���� ȸ�� ����
        if (moveDirection != Vector3.zero)
        {
            // �̵� ������ �ٶ󺸵��� ���ʹϾ� ���
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);

            // ���� ȸ������ ��ǥ ȸ������ �ε巴�� ����
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * 10f);
        }

        // ĳ���� �̵� ���� (������ ���� y�� �ӵ� ����)
        Vector3 currentVelocity = rb.velocity;
        Vector3 newVelocity = moveDirection * moveSpeed;
        rb.velocity = new Vector3(newVelocity.x, currentVelocity.y, newVelocity.z);

        anim.SetBool("Moving", MovementInput != Vector2.zero);
    }

    // InputSystem���� ȣ�� �޴� �Լ�
    public void OnPauseState(InputAction.CallbackContext context)
    {
        // context�� phase(����)�� Performed(�Է���) ������ ��
        if (context.phase == InputActionPhase.Started)
        {
            GameManager.Instance.PauseGame();
        }
    }
}
