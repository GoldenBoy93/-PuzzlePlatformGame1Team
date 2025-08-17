using System.Collections;
using System.Collections.Generic;
using UnityEngine;

// 상태를 구별하기 위한 enum 선언
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
        // Open 상태일때 애니메이터의 파라미터를 true로 설정
        animator.SetBool("DoorOpen", state == DoorState.Open);
    }

    // 외부에서 아래와 같이 호출하여 상태를 변경
    // curInteractGameObject.GetComponent<Door>().SetState();
    public void SetState()
    {
        switch (state)
        {
            // 현재 Oen 상태라면 Closed로 변경
            case DoorState.Open:
                state = DoorState.Closed;
                Debug.Log("Door is now Closed");
                return;

            // 현재 Closed 상태라면 Open으로 변경
            case DoorState.Closed:
                state = DoorState.Open;
                Debug.Log("Door is now Open");
                return;
        }
    }
}
