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
    public float speed = 2f;
    public float runSpeed = 4f;
    public float smooth = 20f;
    public float gravity = -9.81f;

    [Header("Object")]
    public bool toggleCameraRotation;
    public List<GameObject> flashLights;

    CharacterController _controller;
    PlayerInput _input;
    Animator _animator;
    Camera _camera;
    Inventory _inventory;
    UI_ActionKey _uiAction;
    UI_SettingPanel _settingPanel;
    GameObject _crosshair;

    Vector2 dir;
    Vector3 velocity;
    bool isRun;
    bool isGrounded;
    bool toggle = false;


    private void Awake()
    {
        _controller = SafeFetchHelper.GetOrError<CharacterController>(gameObject);
        _animator = SafeFetchHelper.GetOrError<Animator>(gameObject);
        _camera = SafeFetchHelper.GetOrCreateByName<Camera>("Main Camera");
        _input = new PlayerInput();
    }

    private void OnEnable()
    {
        _input.Player.Move.performed += OnMove;
        _input.Player.Move.canceled += OnMove;
        _input.Player.Run.started += ctx => isRun = true;
        _input.Player.Run.canceled += ctx => isRun = false;
        _input.Player.Jump.started += OnJump;
        _input.Player.Flash.started += OnFlash;
        _input.Player.CameraToggle.started += OnToggleCamera;
        _input.Player.Action.performed += OnAction;
        _input.Player.Action.canceled += OnAction;
        _input.Player.Interaction.started += OnInteraction;
        _input.Player.Menu.started += OnMenu;
        _input.Player.Inventory.started += OnInventory;
        _input.Player.PotalGun.started += OnPotalGun;
        _input.Player.MouseL.started += OnMouseL;
        _input.Player.MouseR.started += OnMouseR;
        _input.Player.Enable();
    }
    private void Start()
    {
        _inventory = SafeFetchHelper.GetChildOrError<Inventory>(UI_Manager.Instance.gameObject);
        _uiAction = SafeFetchHelper.GetChildOrError<UI_ActionKey>(UI_Manager.Instance.gameObject);
        _settingPanel = SafeFetchHelper.GetChildOrError<UI_SettingPanel>(UI_Manager.Instance.gameObject);
        _crosshair = UI_Manager.Instance._crosshair;
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
    private void OnDisable() => _input.Player.Disable();

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

    
    public void LockInputOn() => _input.Disable();
    public void LockInputOff() => _input.Enable();

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
    void OnToggleCamera(InputAction.CallbackContext context) //Alt
    {
        if (context.started) // 버튼 눌렀을 때만
            toggleCameraRotation = !toggleCameraRotation; // true ↔ false 토글
    }

    void OnFlash(InputAction.CallbackContext context) // Q
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
    void OnAction(InputAction.CallbackContext context) // Z
    {
        if (context.phase == InputActionPhase.Performed)
        {
            _uiAction.gameObject.SetActive(true);   // 누르는 동안 켜기
            Time.timeScale = 0.2f;
            Cursor.lockState = CursorLockMode.None;
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            _uiAction.gameObject.SetActive(false);  // 떼면 끄기
            Time.timeScale = 1f;
            Cursor.lockState = CursorLockMode.Locked;
        }
    }

    void OnInteraction(InputAction.CallbackContext context) //상호작용키 E
    {
        _animator.SetTrigger("IsInteraction");
    }

    void OnMenu(InputAction.CallbackContext context) // ESC
    {
        if (!context.started) return; // 누를 때만 실행 (뗄 때 무시)
        toggle = !toggle; // 토글

        _settingPanel.OnToggleSettings();
        Time.timeScale = toggle ? 0.1f : 1f;

    }
    void OnInventory(InputAction.CallbackContext context) // Tap
    {
        if (!context.started) return; // 누를 때만 실행 (뗄 때 무시)
        toggle = !toggle; // 토글

        Time.timeScale = toggle ? 0.1f : 1f;


        _inventory.gameObject.SetActive(!_inventory.gameObject.activeSelf);
    }
    void OnPotalGun(InputAction.CallbackContext context) //Ctrl
    {
        if (!context.started) return; // 누를 때만 실행 (뗄 때 무시)
        toggle = !toggle; // 토글

        _animator.SetLayerWeight(2, toggle ? 1f : 0f);
        _animator.SetBool("IsGun", toggle);

        _crosshair.SetActive(!_crosshair.activeSelf);


    }
    void OnMouseL(InputAction.CallbackContext context)
    {
        _animator.SetTrigger("IsShoot");

        //포탈키 1번
    }
    void OnMouseR(InputAction.CallbackContext context)
    {
        _animator.SetTrigger("IsShoot");

        //포탈키 2번

    }
}
