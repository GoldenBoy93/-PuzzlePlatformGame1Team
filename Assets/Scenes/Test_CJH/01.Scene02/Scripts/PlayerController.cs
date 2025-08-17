using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerController : MonoBehaviour //Character Controller ����
{ 
    [Header("Movement")]
    public float speed = 5f;
    public float runSpeed = 8f;
    public float smooth = 10f;
    public float gravity = -9.81f;

    [Header("Object")]
    public bool toggleCameraRotation;
    public GameObject light;
    public GameObject flashLight;

    Animator _animator;
    Camera _camera;
    UnityEngine.CharacterController _controller;
    PlayerInput input;
    Vector2 dir;
    Vector3 velocity;
    bool isRun;
    bool isGrounded;

    private void Awake()
    {
        _animator = GetComponent<Animator>();
        _camera = Camera.main;
        _controller = GetComponent<UnityEngine.CharacterController>();
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
        input.Player.Interaction.started += OnInteraction;
        input.Player.NumberKey.started += OnNumberKey;
        input.Player.Menu.started += OnMenu;
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
        if (!_animator.GetBool("CanMove")) return;

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
    void CanMove()
    {
        bool cur = _animator.GetBool("CanMove");
        _animator.SetBool("CanMove", !cur);
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
            light.SetActive(true);
            flashLight.SetActive(true);
        }
        else if (state == 0) // �⺻ Idle ��ȯ
        {
            _animator.SetInteger("IsFlash", 0);
            _animator.SetLayerWeight(1, 0f);
            light.SetActive(false);
            flashLight.SetActive(false);
        }
    }
    void OnInteraction(InputAction.CallbackContext context) //��ȣ�ۿ�Ű E
    {
        _animator.SetTrigger("IsInteraction");
    }
    void OnNumberKey(InputAction.CallbackContext context)
    {
        if (!_animator.GetBool("CanMove")) return;
        int keyNumber = (int)context.ReadValue<float>();
        switch (keyNumber)
        {
            case 1:
                StartCoroutine(PlayAnimOnce(1)); break;
            case 2:
                StartCoroutine(PlayAnimOnce(2)); break;
            case 3:
                StartCoroutine(PlayAnimOnce(3)); break;
            case 4:
                StartCoroutine(PlayAnimOnce(4)); break;
            case 5:
                StartCoroutine(PlayAnimOnce(5)); break;
            case 6:
                StartCoroutine(PlayAnimOnce(6)); break;
        }
    }
    IEnumerator PlayAnimOnce(int animValue)
    {
        _animator.SetInteger("animBaseInt", animValue); // �ִϸ��̼� ����
        yield return new WaitForSeconds(1f); // �ణ ����
        _animator.SetInteger("animBaseInt", 0); // �ٽ� 0���� ����
    }
    void OnMenu(InputAction.CallbackContext context)
    {

    }
}
