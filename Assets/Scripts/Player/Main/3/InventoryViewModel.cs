using System;
using System.Collections;
using System.Collections.Generic;
using UniRx;
using UnityEngine;

// 슬롯을 ReactiveProperty로 감싸는 ViewModel
public class ItemSlotViewModel
{
    public ReactiveProperty<InventorySlot> Slot { get; }

    public ItemSlotViewModel(InventorySlot slot)
    {
        Slot = new ReactiveProperty<InventorySlot>(slot);
    }

    public string LabelText => Slot.Value.IsEmpty ? "" : $"{Slot.Value.ItemId} x{Slot.Value.Quantity}" + (Slot.Value.Equipped ? " [E]" : "");
}
// 인벤토리 전체 ViewModel
public class InventoryViewModel : IDisposable
{
    public List<ItemSlotViewModel> Slots { get; }
    public ReactiveProperty<int?> EquippedIndex { get; }

    private InventoryModel model;

    public InventoryViewModel(InventoryModel model)
    {
        this.model = model;
        Slots = new List<ItemSlotViewModel>();
        foreach (var s in model.Slots)
            Slots.Add(new ItemSlotViewModel(s));

        EquippedIndex = new ReactiveProperty<int?>(null);
    }

    public void AddItem(string itemId, int amount, int maxStack)
    {
        model.AddStackable(itemId, amount, maxStack);
        RefreshSlots();
    }

    public void RemoveAt(int index, int amount = 1)
    {
        if (model.RemoveAt(index, amount))
            Slots[index].Slot.Value = model.Slots[index];
    }

    public void Equip(int index)
    {
        model.Equip(index);
        EquippedIndex.Value = index;
        RefreshSlots();
    }

    public void UnEquip(int index)
    {
        model.UnEquip(index);
        EquippedIndex.Value = null;
        Slots[index].Slot.Value = model.Slots[index];
    }

    private void RefreshSlots()
    {
        for (int i = 0; i < Slots.Count; i++)
            Slots[i].Slot.Value = model.Slots[i];
    }

    public void Dispose()
    {
        foreach (var s in Slots)
            s.Slot.Dispose();
        EquippedIndex?.Dispose();
    }
}