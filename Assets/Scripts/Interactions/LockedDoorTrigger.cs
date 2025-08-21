using System.Linq;
using TMPro;
using UnityEngine;

public class LockedDoorTrigger : MonoBehaviour
{
    public InventoryViewModel inventoryVM; // �÷��̾� �κ��丮
    public ItemType requiredKey;           // �� ���� ���� Ű
    private bool isOpen = false;
    public TextMeshProUGUI text;

    private void OnTriggerEnter(Collider other)
    {
        if (isOpen) return;
        if (!other.CompareTag("Player")) return;

        // �÷��̾� �κ��丮�� requiredKey �ִ��� üũ
        bool hasKey = inventoryVM.Slots
            .Any(s => s.Slot.Item.Value != null && s.Slot.Item.Value.type == requiredKey);

        if (hasKey)
        {
            Open();
        }
        else
        {
            text.text = ($"���� ���� ���� {requiredKey} Ű�� �ʿ��մϴ�.");
        }
    }

    private void Open()
    {
        isOpen = true;
        text.text = ($"{requiredKey} Ű�� �� ����!");
        // �ʿ� �� �ִϸ��̼�, ����, �浹ü ��Ȱ��ȭ �� ó��
        Destroy(gameObject); // �� ������Ʈ ����
    }
}