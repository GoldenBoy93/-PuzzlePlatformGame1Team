using UnityEngine;

public class ItemObject : MonoBehaviour
{
    public ItemData data;

    public string GetInteractPrompt()
    {
        string str = $"{data.displayName}\n{data.description}";
        return str;
    }

    // �Ű������� ���� ��ȣ�ۿ� ���� ���� ������Ʈ�� ����
    public void OnInteract(GameObject interactor)
    {
        if (data == null) return;

        // �κ��丮 ViewModel ��������
        var viewModel2 = UI_Manager.Instance._viewModel2;
        if (viewModel2 != null)
        {
            // stackable�̶�� maxStack �����ͼ� �߰�
            int maxStack = data.maxStack; // ItemData�� maxStack �ʵ尡 �ִٰ� ����
            viewModel2.AddItem(data.id, 1);
        }

        // ������ ���忡�� ����
        Destroy(gameObject);
    }


    //else if (curInteractGameObject.GetComponent<Door>() != null)
    //{
    //    // ���� ��ȣ�ۿ� ���� ������Ʈ�� Door ������Ʈ�� ������ �ִٸ�,
    //    // Door�� SetState �޼��带 ȣ���Ͽ� ���¸� ����
    //    curInteractGameObject.GetComponent<Door>().SetState();
    //}

}