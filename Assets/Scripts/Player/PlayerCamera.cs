using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    // 카메라가 따라다닐 타겟 (플레이어)
    public Transform target;
    // 카메라와 타겟 사이의 거리
    public float distance = 5.0f;
    // 카메라 회전 속도
    public float lookSpeed = 2.0f;

    private float _currentX = 0.0f;
    private float _currentY = 0.0f;

    // Input System에서 받아올 마우스 입력값
    private Vector2 _mouseDelta;
    private bool _isRightMouseButtonPressed = false;

    void Start()
    {
        // 마우스 커서를 숨기고 잠금
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Input System으로 마우스 우클릭 상태를 받아옴
    public void OnRightClick(InputAction.CallbackContext context)
    {
        if (context.phase == InputActionPhase.Started)
        {
            _isRightMouseButtonPressed = true;
        }
        else if (context.phase == InputActionPhase.Canceled)
        {
            _isRightMouseButtonPressed = false;
        }
    }

    // Input System으로 마우스 움직임을 받아옴
    public void OnLook(InputAction.CallbackContext context)
    {
        _mouseDelta = context.ReadValue<Vector2>();
    }

    void LateUpdate()
    {
        // 마우스 우클릭 중일 때만 카메라 회전
        if (_isRightMouseButtonPressed)
        {
            // 마우스 입력 값을 이용해 회전 각도 계산
            _currentX += _mouseDelta.x * lookSpeed;
            _currentY -= _mouseDelta.y * lookSpeed;

            // 상하 시야각 제한 (옵션)
            _currentY = Mathf.Clamp(_currentY, -30f, 60f);

            // 마우스 커서 다시 잠금
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            // 마우스 우클릭을 하지 않을 때는 커서 보이기 (필요에 따라)
            Cursor.lockState = CursorLockMode.None;
        }

        // 카메라 회전 적용
        Quaternion rotation = Quaternion.Euler(_currentY, _currentX, 0);

        // 타겟의 위치를 기준으로 회전
        Vector3 newPosition = target.position - rotation * Vector3.forward * distance;

        // 카메라의 위치와 회전을 최종 적용
        transform.position = newPosition;
        transform.rotation = rotation;
    }
}