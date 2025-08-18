using UnityEngine;
using UnityEngine.InputSystem;

public class InputProbe : MonoBehaviour
{
    void Start()
    {
        var pi = GetComponent<PlayerInput>();
        if (pi == null)
        {
            Debug.LogError(" PlayerInput 컴포넌트가 없습니다!");
            return;
        }

        var act = pi.actions["Move"];    // 액션 이름 정확히 "Move"
        if (act == null)
        {
            Debug.LogError(" 'Move' 액션을 찾을 수 없습니다!");
            return;
        }

        act.Enable();
        act.performed += ctx => Debug.Log("[Move.performed] " + ctx.ReadValue<Vector2>());
        act.canceled += ctx => Debug.Log("[Move.canceled]");
    }
}
