using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public ItemData data;

    public string GetInteractPrompt()
    {
        if (data == null)
        {
            string str = $"{data.displayName}\n{data.description}";
            return str;
        }
        return null;
    }

    // 매개변수로 현재 상호작용 중인 게임 오브젝트를 받음
    public void OnInteract(GameObject interactor)
    {
        if (data == null) return;

        // ViewModel에 아이템 추가
        var viewModel = UI_Manager.Instance._viewModel;
        //viewModel.AddItem(data, 1);

        Destroy(gameObject);
    }



    //else if (curInteractGameObject.GetComponent<Door>() != null)
    //{
    //    // 현재 상호작용 게임 오브젝트가 Door 컴포넌트를 가지고 있다면,
    //    // Door의 SetState 메서드를 호출하여 상태를 변경
    //    curInteractGameObject.GetComponent<Door>().SetState();
    //}

}