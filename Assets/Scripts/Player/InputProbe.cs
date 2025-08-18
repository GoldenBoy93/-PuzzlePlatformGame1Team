using UnityEngine;
using UnityEngine.InputSystem;

public class InputProbe : MonoBehaviour
{
    void Start()
    {
        var pi = GetComponent<PlayerInput>();
        if (pi == null)
        {
            Debug.LogError(" PlayerInput ������Ʈ�� �����ϴ�!");
            return;
        }

        var act = pi.actions["Move"];    // �׼� �̸� ��Ȯ�� "Move"
        if (act == null)
        {
            Debug.LogError(" 'Move' �׼��� ã�� �� �����ϴ�!");
            return;
        }

        act.Enable();
        act.performed += ctx => Debug.Log("[Move.performed] " + ctx.ReadValue<Vector2>());
        act.canceled += ctx => Debug.Log("[Move.canceled]");
    }
}
