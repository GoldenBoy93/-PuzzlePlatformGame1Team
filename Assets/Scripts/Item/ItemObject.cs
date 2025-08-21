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
    public void OnInteract(GameObject interactor)
    {
        if (data == null) return;

        // PlayerModel 참조 가져오기
        var playerModel = UI_Manager.Instance._model;

        // InventoryModel에 저장
        playerModel.Inventory.AddItem(data, 1);

        // 옵저버/이벤트로 UI 갱신 (예: UI_Manager에게 알리기)
        UI_Manager.Instance._inventory.RefreshUI();

        Destroy(gameObject);
    }



    //else if (curInteractGameObject.GetComponent<Door>() != null)
    //{
    //    // 현재 상호작용 게임 오브젝트가 Door 컴포넌트를 가지고 있다면,
    //    // Door의 SetState 메서드를 호출하여 상태를 변경
    //    curInteractGameObject.GetComponent<Door>().SetState();
    //}

}