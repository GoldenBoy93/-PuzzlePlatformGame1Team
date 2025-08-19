using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UniRx;

public class InventoryView : MonoBehaviour
{
    public GameObject inventoryWindow;
    public Transform slotPanel;
    public UI_ItemSlot slotPrefab;
    public Transform dropPosition;

    public TextMeshProUGUI selectedItemName;
    public TextMeshProUGUI selectedItemDescription;
    public GameObject useButton;
    public GameObject dropButton;

    private PlayerViewModel viewModel;
    private UI_ItemSlot[] slots;
    private string selectedItemNameKey;

    private void Start()
    {
        viewModel = GameManager.Instance.UIManager.ViewModel; // 예시

        // 슬롯 초기화
        slots = new UI_ItemSlot[slotPanel.childCount];
        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = slotPanel.GetChild(i).GetComponent<UI_ItemSlot>();
            slots[i].index = i;
            slots[i].inventoryView = this;
            slots[i].Clear();
        }

        inventoryWindow.SetActive(false);
        ClearSelection();

        // Reactive 구독
        viewModel.Inventory.ObserveAdd().Subscribe(e => OnItemUpdated(e.Key, e.Value));
        viewModel.Inventory.ObserveReplace().Subscribe(e => OnItemUpdated(e.Key, e.NewValue));
        viewModel.Inventory.ObserveRemove().Subscribe(e => OnItemRemoved(e.Key));
    }

    public void ToggleInventory()
    {
        inventoryWindow.SetActive(!inventoryWindow.activeSelf);
    }

    private void OnItemUpdated(string name, int amount)
    {
        var slot = GetSlotWithItem(name) ?? GetEmptySlot();
        if (slot != null)
            slot.Set(name, amount);
    }

    private void OnItemRemoved(string name)
    {
        var slot = GetSlotWithItem(name);
        if (slot != null)
        {
            slot.Clear();
            if (selectedItemNameKey == name)
                ClearSelection();
        }
    }

    private UI_ItemSlot GetSlotWithItem(string name)
    {
        foreach (var slot in slots)
            if (slot.itemName == name) return slot;
        return null;
    }

    private UI_ItemSlot GetEmptySlot()
    {
        foreach (var slot in slots)
            if (string.IsNullOrEmpty(slot.itemName)) return slot;
        return null;
    }

    public void SelectItem(int index)
    {
        var slot = slots[index];
        if (string.IsNullOrEmpty(slot.itemName)) return;

        selectedItemNameKey = slot.itemName;
        selectedItemName.text = selectedItemNameKey;
        //selectedItemDescription.text = slot.GetItemData(selectedItemNameKey).description;

        useButton.SetActive(true);
        dropButton.SetActive(true);
    }

    private void ClearSelection()
    {
        selectedItemNameKey = null;
        selectedItemName.text = "";
        selectedItemDescription.text = "";
        useButton.SetActive(false);
        dropButton.SetActive(false);
    }

    public void OnUseButton()
    {
        if (string.IsNullOrEmpty(selectedItemNameKey)) return;

        // 예시: 체력 회복
        viewModel.Heal(viewModel.Inventory[selectedItemNameKey]);
        viewModel.RemoveItem(selectedItemNameKey, 1);
        ClearSelection();
    }

    public void OnDropButton()
    {
        if (string.IsNullOrEmpty(selectedItemNameKey)) return;

        // 드롭 위치에 아이템 생성
        //var data = slots[0].GetItemData(selectedItemNameKey); // 임시
        //Instantiate(data.dropPrefab, dropPosition.position, Quaternion.identity);

        viewModel.RemoveItem(selectedItemNameKey, 1);
        ClearSelection();
    }
}
