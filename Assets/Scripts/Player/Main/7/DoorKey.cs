using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class DoorKey : MonoBehaviour
{
    public ItemData keyItem; // ScriptableObject
    public InventoryViewModel inventoryVM;
    public TextMeshProUGUI text;

    private bool pickedUp = false;

    private void OnTriggerEnter(Collider other)
    {
        if (pickedUp) return;
        if (!other.CompareTag("Player")) return;

        inventoryVM.AddItem(keyItem, 1); // Ű ȹ��
        pickedUp = true;
        Destroy(gameObject); // Ű ������Ʈ ����
        text.text = ($"{keyItem.displayName} ȹ��!");
    }
}
