using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;

public class InventoryView : MonoBehaviour
{
    public Transform slotPanel;
    public GameObject slotPrefab;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public GameObject useButton;
    public GameObject equipButton;
    public GameObject unequipButton;
    public GameObject dropButton;

    private InventoryViewModel viewModel;
    private ItemSlot[] slots;
    private ReactiveProperty<int?> selectedIndex = new ReactiveProperty<int?>(null);
    private CompositeDisposable disposables = new CompositeDisposable();

    public void Init(InventoryViewModel vm)
    {
        viewModel = vm;

        slots = new ItemSlot[viewModel.Slots.Count];
        for (int i = 0; i < slots.Length; i++)
        {
            var go = Instantiate(slotPrefab, slotPanel);
            slots[i] = go.GetComponent<ItemSlot>();
            slots[i].index = i;
            slots[i].Init(viewModel.Slots[i], OnSelectItem);
        }

        // Ã¹ ½½·Ô ¼±ÅÃ
        if (slots.Length > 0)
            OnSelectItem(0);
    }

    private void OnSelectItem(int index)
    {
        selectedIndex.Value = index;
        UpdateSelectedUI(index);
    }

    private void UpdateSelectedUI(int index)
    {
        var slot = viewModel.Slots[index].Slot;

        nameText.text = string.IsNullOrEmpty(slot.ItemId.Value) ? "" : slot.ItemId.Value;
        descriptionText.text = $"Quantity: {slot.Quantity.Value}";

        useButton.SetActive(!string.IsNullOrEmpty(slot.ItemId.Value) && slot.ItemId.Value.Contains("Consumable"));
        equipButton.SetActive(!slot.IsEmpty.Value && !slot.Equipped.Value);
        unequipButton.SetActive(slot.Equipped.Value);
        dropButton.SetActive(!slot.IsEmpty.Value);
    }

    public void OnEquipButton()
    {
        if (selectedIndex.Value == null) return;
        viewModel.Equip(selectedIndex.Value.Value);
    }

    public void OnUnEquipButton()
    {
        if (selectedIndex.Value == null) return;
        viewModel.UnEquip(selectedIndex.Value.Value);
    }

    public void OnDropButton()
    {
        if (selectedIndex.Value == null) return;
        viewModel.RemoveAt(selectedIndex.Value.Value);
    }

    private void OnDestroy() => disposables.Dispose();
}