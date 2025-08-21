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

    // �Ű������� ���� ��ȣ�ۿ� ���� ���� ������Ʈ�� ����
    public void OnInteract(GameObject interactor)
    {
        if (data == null) return;

        // ViewModel�� ������ �߰�
        var viewModel = UI_Manager.Instance._viewModel;
        //viewModel.AddItem(data, 1);

        Destroy(gameObject);
    }



    //else if (curInteractGameObject.GetComponent<Door>() != null)
    //{
    //    // ���� ��ȣ�ۿ� ���� ������Ʈ�� Door ������Ʈ�� ������ �ִٸ�,
    //    // Door�� SetState �޼��带 ȣ���Ͽ� ���¸� ����
    //    curInteractGameObject.GetComponent<Door>().SetState();
    //}

}