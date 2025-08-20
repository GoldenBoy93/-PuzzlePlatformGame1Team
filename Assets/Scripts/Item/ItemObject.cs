using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public ItemData data;

    public string GetInteractPrompt()
    {
        string str = $"{data.displayName}\n{data.description}";
        return str;
    }

    // 매개변수로 현재 상호작용 중인 게임 오브젝트를 받음
    public void OnInteract(GameObject curInteractGameObject)
    {
        //// 주울 수 있는 아이템이라면,
        //if (data.isGetable)
        //{
        //    //Player 스크립트에 상호작용 아이템 data 넘기기.
        //    GameManager.Instance.Player.itemData = data;
        //    GameManager.Instance.Player.addItem?.Invoke();
        //    Destroy(gameObject);
        //}
        //else if (curInteractGameObject.GetComponent<Door>() != null)
        //{
        //    // 현재 상호작용 게임 오브젝트가 Door 컴포넌트를 가지고 있다면,
        //    // Door의 SetState 메서드를 호출하여 상태를 변경
        //    curInteractGameObject.GetComponent<Door>().SetState();
        //}

        //return;
    }
}