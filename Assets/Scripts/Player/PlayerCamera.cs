using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerCamera : MonoBehaviour
{
    // ī�޶� ����ٴ� Ÿ�� (�÷��̾�)
    public Transform target;
    // ī�޶�� Ÿ�� ������ �Ÿ�
    public float distance = 5.0f;
    // ī�޶� ȸ�� �ӵ�
    public float lookSpeed = 2.0f;

    private float _currentX = 0.0f;
    private float _currentY = 0.0f;

    // Input System���� �޾ƿ� ���콺 �Է°�
    private Vector2 _mouseDelta;
    private bool _isRightMouseButtonPressed = false;

    void Start()
    {
        // ���콺 Ŀ���� ����� ���
        Cursor.lockState = CursorLockMode.Locked;
    }

    // Input System���� ���콺 ��Ŭ�� ���¸� �޾ƿ�
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

    // Input System���� ���콺 �������� �޾ƿ�
    public void OnLook(InputAction.CallbackContext context)
    {
        _mouseDelta = context.ReadValue<Vector2>();
    }

    void LateUpdate()
    {
        // ���콺 ��Ŭ�� ���� ���� ī�޶� ȸ��
        if (_isRightMouseButtonPressed)
        {
            // ���콺 �Է� ���� �̿��� ȸ�� ���� ���
            _currentX += _mouseDelta.x * lookSpeed;
            _currentY -= _mouseDelta.y * lookSpeed;

            // ���� �þ߰� ���� (�ɼ�)
            _currentY = Mathf.Clamp(_currentY, -30f, 60f);

            // ���콺 Ŀ�� �ٽ� ���
            Cursor.lockState = CursorLockMode.Locked;
        }
        else
        {
            // ���콺 ��Ŭ���� ���� ���� ���� Ŀ�� ���̱� (�ʿ信 ����)
            Cursor.lockState = CursorLockMode.None;
        }

        // ī�޶� ȸ�� ����
        Quaternion rotation = Quaternion.Euler(_currentY, _currentX, 0);

        // Ÿ���� ��ġ�� �������� ȸ��
        Vector3 newPosition = target.position - rotation * Vector3.forward * distance;

        // ī�޶��� ��ġ�� ȸ���� ���� ����
        transform.position = newPosition;
        transform.rotation = rotation;
    }
}