using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.InputSystem;

public class PlayerController2 : MonoBehaviour
{
    [Header("Movement")]
    private Vector2 MovementInput; // 입력값 받을 Vector3 변수 선언
    public float moveSpeed = 1f;
    public float jumpPower = 1f;

    [HideInInspector]
    Rigidbody rb;
    Animator anim;
    Transform cameraTransform;

    void Start()
    {
        // GameManager의 PlayerInstance를 통해 playerRigidbody에 접근하여 변수에 저장
        rb = GameManager.Instance.PlayerInstance.playerRigidbody;
        anim = GameManager.Instance.PlayerInstance.playerAnimator;
    }

    void FixedUpdate()
    {
        // 메인 카메라의 transform 참조
        cameraTransform = Camera.main.transform;

        // FixedUpdate에서 Move() 함수를 호출하여 DeltaTime이 적용 된 이동
        Move();
    }

    // InputSystem에서 호출 받는 함수
    public void OnMoveInput(InputAction.CallbackContext context)
    {
        // context의 phase(상태)가 Performed(입력중) 상태일 때
        if (context.phase == InputActionPhase.Performed)
        {
            // context의 Vector2 값을 할당
            MovementInput = context.ReadValue<Vector2>();
        }

        // context의 phase(상태)가 Performed(입력취소) 상태일 때
        else if (context.phase == InputActionPhase.Canceled)
        {
            // Vector2(0,0)을 할당
            MovementInput = Vector2.zero;
        }
    }

    // InputSystem에서 호출 받는 함수
    public void OnJumpInput(InputAction.CallbackContext context)
    {
        // context의 phase(상태)가 Performed(입력중) 상태일 때
        if (context.phase == InputActionPhase.Started)
        {
            // context의 Vector2 값을 할당
            rb.AddForce(Vector2.up * jumpPower, ForceMode.Impulse);
            
            anim.SetTrigger("Jumping");
        }
    }

    void Move()
    {
        // 카메라의 앞 방향과 오른쪽 방향 벡터를 가져옴
        // y축을 0으로 만들어 수평 방향 벡터만 사용
        Vector3 cameraForward = Vector3.Scale(cameraTransform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 cameraRight = Vector3.Scale(cameraTransform.right, new Vector3(1, 0, 1)).normalized;

        // 카메라 시점 기준으로 이동 방향 계산
        Vector3 moveDirection = (cameraForward * MovementInput.y + cameraRight * MovementInput.x).normalized;

        // 캐릭터 회전 로직
        if (moveDirection != Vector3.zero)
        {
            // 이동 방향을 바라보도록 쿼터니언 계산
            Quaternion targetRotation = Quaternion.LookRotation(moveDirection);

            // 현재 회전에서 목표 회전으로 부드럽게 보간
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.fixedDeltaTime * 10f);
        }

        // 캐릭터 이동 로직 (점프를 위해 y축 속도 유지)
        Vector3 currentVelocity = rb.velocity;
        Vector3 newVelocity = moveDirection * moveSpeed;
        rb.velocity = new Vector3(newVelocity.x, currentVelocity.y, newVelocity.z);

        anim.SetBool("Moving", MovementInput != Vector2.zero);
    }

    // InputSystem에서 호출 받는 함수
    public void OnPauseState(InputAction.CallbackContext context)
    {
        // context의 phase(상태)가 Performed(입력중) 상태일 때
        if (context.phase == InputActionPhase.Started)
        {
            GameManager.Instance.PauseGame();
        }
    }
}
