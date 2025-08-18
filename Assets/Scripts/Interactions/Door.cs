using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// ���¸� �����ϱ� ���� enum ����
public enum DoorState
{
    Closed,
    Open,
}

public class Door : MonoBehaviour
{
    private DoorState state;
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    private void Update()
    {
        // Open �����϶� �ִϸ������� �Ķ���͸� true�� ����
        animator.SetBool("DoorOpen", state == DoorState.Open);
    }

    // �ܺο��� �Ʒ��� ���� ȣ���Ͽ� ���¸� ����
    // curInteractGameObject.GetComponent<Door>().SetState();
    public void SetState()
    {
        switch (state)
        {
            // ���� Oen ���¶�� Closed�� ����
            case DoorState.Open:
                state = DoorState.Closed;
                Debug.Log("Door is now Closed");
                return;

            // ���� Closed ���¶�� Open���� ����
            case DoorState.Closed:
                state = DoorState.Open;
                Debug.Log("Door is now Open");
                return;
        }
    }
}
