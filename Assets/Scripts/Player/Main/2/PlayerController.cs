using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public partial class PlayerController : MonoBehaviour //Character Controller 전용
{ 
    [Header("Movement")]
    float speed = 2f;
    float runSpeed = 4f;
    float smooth = 20f;
    float gravity = -9.81f;

    [Header("Camera")]

    [Header("Object")]
    public bool toggleCameraRotation;
    public GameObject uiAction;
    public List<GameObject> flashLights;

    Animator _animator;
    Camera _camera;
    CharacterController _controller;
    PlayerInput input;

    Vector2 dir;
    Vector3 velocity;
    bool isRun;
    bool isGrounded;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _camera = Camera.main;
        _controller = GetComponent<CharacterController>();
        input = new PlayerInput();
    }
    private void OnEnable()
    {
        input.Player.Move.performed += OnMove;
        input.Player.Move.canceled += OnMove;
        input.Player.Run.started += ctx => isRun = true;
        input.Player.Run.canceled += ctx => isRun = false;
        input.Player.Jump.started += OnJump;
        input.Player.Flash.started += OnFlash;
        input.Player.CameraToggle.started += OnToggleCamera;
        input.Player.Action.performed += OnAction;
        input.Player.Action.canceled += OnAction;
        input.Player.Interaction.started += OnInteraction;
        input.Player.Menu.started += OnMenu;
        input.Player.PotalGun.started += OnPotalGun;
        input.Player.MouseL.started += OnMouseL;
        input.Player.MouseR.started += OnMouseR;
        input.Player.Enable();
    }
    private void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }
    private void Update()
    {
        isGrounded = _controller.isGrounded; //바닦체크
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;
        velocity.y += gravity * Time.deltaTime; //중력 적용
        _controller.Move(velocity * Time.deltaTime);
        Move();
    }
    private void LateUpdate()
    {
        if(toggleCameraRotation)
        {
            Vector3 playerRotate = Vector3.Scale(_camera.transform.forward, new Vector3(1, 0, 1));
            transform.rotation = Quaternion.Slerp
                (transform.rotation, Quaternion.LookRotation(playerRotate), Time.deltaTime * smooth);
        }
    }
    private void OnDisable() => input.Player.Disable();

    void Move()
    {
        float finalSpeed = isRun ? runSpeed : speed;

        // 카메라 기준 forward/right
        Vector3 camForward = Vector3.Scale(_camera.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 camRight = Vector3.Scale(_camera.transform.right, new Vector3(1, 0, 1)).normalized;
        // 입력 기반 이동 방향
        Vector3 moveDir = camForward * dir.y + camRight * dir.x;
        moveDir.Normalize();

        Vector3 move = moveDir * finalSpeed + new Vector3(0, velocity.y, 0);
        _controller.Move(move * Time.deltaTime);
        // 캐릭터 회전 (이동 방향으로)

        if (moveDir.magnitude > 0.1f)
        {
            Quaternion targetRot = Quaternion.LookRotation(moveDir);
            transform.rotation = Quaternion.Slerp(transform.rotation, targetRot, Time.deltaTime * smooth);
        }

        float percent = (isRun ? 1 : 0.5f) * new Vector2(dir.x, dir.y).magnitude;
        _animator.SetFloat("Blend", percent, 0.3f, Time.deltaTime);
    }

    
    public void LockInputOn() => input.Disable();
    public void LockInputOff() => input.Enable();

    void OnMove(InputAction.CallbackContext context)
    {
        dir = context.ReadValue<Vector2>();
    }
    void OnJump(InputAction.CallbackContext context)
    {
        if (isGrounded && !_animator.GetCurrentAnimatorStateInfo(0).IsName("Jump"))
        {
            _animator.SetTrigger("IsJump");
            velocity.y = Mathf.Sqrt(-gravity);
        }
    }
    void OnToggleCamera(InputAction.CallbackContext context)
    {
        if (context.started) // 버튼 눌렀을 때만
            toggleCameraRotation = !toggleCameraRotation; // true ↔ false 토글
    }

    void OnFlash(InputAction.CallbackContext context)
    {
        int state = _animator.GetInteger("IsFlash");

        if (state == 0) // 맨몸 상태 → 꺼내기
            _animator.SetInteger("IsFlash", 1);
        else if (state == 2) // 무기 장착 Idle 상태 → 집어넣기
            _animator.SetInteger("IsFlash", 3);
    }
    void EquipFlash(int state)
    {
        if (state == 1) // 무기 장착 Idle 전환
        {
            _animator.SetInteger("IsFlash", 2);
            _animator.SetLayerWeight(1, 1f);
            _animator.CrossFadeInFixedTime("Blend_Flash", 0.2f, 1, 0f);
            foreach (var light in flashLights)
                light.SetActive(true);
        }
        else if (state == 0) // 기본 Idle 전환
        {
            _animator.SetInteger("IsFlash", 0);
            _animator.SetLayerWeight(1, 0f);
            foreach (var light in flashLights)
                light.SetActive(false);
        }
    }
    void OnAction(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Performed)
        {
            uiAction.SetActive(true);   // 누르는 동안 켜기
            Time.timeScale = 0.2f;
            Cursor.lockState = CursorLockMode.None;
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            uiAction.SetActive(false);  // 떼면 끄기
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    void OnInteraction(InputAction.CallbackContext context) //상호작용키 E
    {
        _animator.SetTrigger("IsInteraction");
    }

    void OnMenu(InputAction.CallbackContext context)
    {

    }
    void OnPotalGun(InputAction.CallbackContext context)
    {

    }
    void OnMouseL(InputAction.CallbackContext context)
    {

    }
    void OnMouseR(InputAction.CallbackContext context)
    {

    }

}
