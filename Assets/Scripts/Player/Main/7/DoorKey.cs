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

        inventoryVM.AddItem(keyItem, 1); // ≈∞ »πµÊ
        pickedUp = true;
        Destroy(gameObject); // ≈∞ ø¿∫Í¡ß∆Æ ¡¶∞≈
        text.text = ($"{keyItem.displayName} »πµÊ!");
    }
}
