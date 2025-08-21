using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;

public class InventoryView : MonoBehaviour
{
    public Transform slotPanel;
    public GameObject slotPrefab;
    public TextMeshProUGUI name;
    public TextMeshProUGUI description;
    public GameObject useButton;
    public GameObject equipButton;
    public GameObject unequipButton;
    public GameObject dropButton;

    private InventoryViewModel viewModel;
    private ItemSlot[] slots;
    private int selectedIndex = -1;

    private CompositeDisposable disposables = new CompositeDisposable();

    public void Init(InventoryViewModel vm)
    {
        viewModel = vm;

        // 슬롯 생성
        slots = new ItemSlot[viewModel.Slots.Count];
        for (int i = 0; i < slots.Length; i++)
        {
            var go = Instantiate(slotPrefab, slotPanel);
            slots[i] = go.GetComponent<ItemSlot>();
            slots[i].index = i;
            slots[i].Init(viewModel.Slots[i], SelectItem);
            slots[i].Clear();
        }

        RefreshSlots();
    }

    // 슬롯 UI 전체 갱신
    public void RefreshSlots()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            var slotData = viewModel.Slots[i].Slot.Value;

            if (slotData.IsEmpty)
            {
                slots[i].Clear();
            }
            else
            {
                slots[i].Clear();
            }
        }
    }
    public void SelectItem(int index)
    {
        if (viewModel.Slots[index].Slot.Value.IsEmpty) return;

        selectedIndex = index;
        var slotData = viewModel.Slots[index].Slot.Value;

        name.text = slotData.ItemId;
        description.text = $"Quantity: {slotData.Quantity}";

        useButton.SetActive(slotData.ItemId.Contains("Consumable")); // 타입 체크
        equipButton.SetActive(!slotData.IsEmpty);
        unequipButton.SetActive(slotData.Equipped);
        dropButton.SetActive(!slotData.IsEmpty);
    }
    public void OnEquipButton()
    {
        if (selectedIndex < 0) return;
        viewModel.Equip(selectedIndex);
    }

    public void OnUnEquipButton()
    {
        if (selectedIndex < 0) return;
        viewModel.UnEquip(selectedIndex);
    }

    public void OnDropButton()
    {
        if (selectedIndex < 0) return;
        viewModel.RemoveAt(selectedIndex, 1);
    }

    void OnDestroy() => disposables.Dispose();
}