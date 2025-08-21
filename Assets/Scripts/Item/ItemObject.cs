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

        // 인벤토리 ViewModel 가져오기
        var viewModel2 = UI_Manager.Instance._viewModel2;
        if (viewModel2 != null)
        {
            // stackable이라면 maxStack 가져와서 추가
            int maxStack = data.maxStack; // ItemData에 maxStack 필드가 있다고 가정
            viewModel2.AddItem(data.id, 1);
        }

        // 아이템 월드에서 제거
        Destroy(gameObject);
    }


    //else if (curInteractGameObject.GetComponent<Door>() != null)
    //{
    //    // 현재 상호작용 게임 오브젝트가 Door 컴포넌트를 가지고 있다면,
    //    // Door의 SetState 메서드를 호출하여 상태를 변경
    //    curInteractGameObject.GetComponent<Door>().SetState();
    //}

}