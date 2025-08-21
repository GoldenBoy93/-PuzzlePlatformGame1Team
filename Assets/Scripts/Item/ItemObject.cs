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

        // PlayerModel ���� ��������
        var playerModel = UI_Manager.Instance._model;

        // InventoryModel�� ����
        playerModel.Inventory.AddItem(data, 1);

        // ������/�̺�Ʈ�� UI ���� (��: UI_Manager���� �˸���)
        UI_Manager.Instance._inventory.RefreshUI();

        Destroy(gameObject);
    }



    //else if (curInteractGameObject.GetComponent<Door>() != null)
    //{
    //    // ���� ��ȣ�ۿ� ���� ������Ʈ�� Door ������Ʈ�� ������ �ִٸ�,
    //    // Door�� SetState �޼��带 ȣ���Ͽ� ���¸� ����
    //    curInteractGameObject.GetComponent<Door>().SetState();
    //}

}