using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.InputSystem.Controls;

public partial class PlayerController : MonoBehaviour //Character Controller ����
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
        isGrounded = _controller.isGrounded; //�ٴ�üũ
        if (isGrounded && velocity.y < 0)
            velocity.y = -2f;
        velocity.y += gravity * Time.deltaTime; //�߷� ����
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

        // ī�޶� ���� forward/right
        Vector3 camForward = Vector3.Scale(_camera.transform.forward, new Vector3(1, 0, 1)).normalized;
        Vector3 camRight = Vector3.Scale(_camera.transform.right, new Vector3(1, 0, 1)).normalized;
        // �Է� ��� �̵� ����
        Vector3 moveDir = camForward * dir.y + camRight * dir.x;
        moveDir.Normalize();

        Vector3 move = moveDir * finalSpeed + new Vector3(0, velocity.y, 0);
        _controller.Move(move * Time.deltaTime);
        // ĳ���� ȸ�� (�̵� ��������)

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
        if (context.started) // ��ư ������ ����
            toggleCameraRotation = !toggleCameraRotation; // true �� false ���
    }

    void OnFlash(InputAction.CallbackContext context)
    {
        int state = _animator.GetInteger("IsFlash");

        if (state == 0) // �Ǹ� ���� �� ������
            _animator.SetInteger("IsFlash", 1);
        else if (state == 2) // ���� ���� Idle ���� �� ����ֱ�
            _animator.SetInteger("IsFlash", 3);
    }
    void EquipFlash(int state)
    {
        if (state == 1) // ���� ���� Idle ��ȯ
        {
            _animator.SetInteger("IsFlash", 2);
            _animator.SetLayerWeight(1, 1f);
            _animator.CrossFadeInFixedTime("Blend_Flash", 0.2f, 1, 0f);
            foreach (var light in flashLights)
                light.SetActive(true);
        }
        else if (state == 0) // �⺻ Idle ��ȯ
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
            uiAction.SetActive(true);   // ������ ���� �ѱ�
            Time.timeScale = 0.2f;
            Cursor.lockState = CursorLockMode.None;
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            uiAction.SetActive(false);  // ���� ����
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    void OnInteraction(InputAction.CallbackContext context) //��ȣ�ۿ�Ű E
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
