using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

// ½½·ÔÀ» ReactiveProperty·Î °¨½Î´Â ViewModel
public class ItemSlotViewModel : IDisposable
{
    public InventorySlot Slot { get; }
    public ReadOnlyReactiveProperty<string> LabelText { get; }

    private CompositeDisposable disposables = new CompositeDisposable();

    public ItemSlotViewModel(InventorySlot slot)
    {
        Slot = slot;

        LabelText = Observable.CombineLatest(
            Slot.ItemId, Slot.Quantity, Slot.Equipped,
            (id, qty, eq) => string.IsNullOrEmpty(id) ? "" : $"{id} x{qty}" + (eq ? " [E]" : "")
        ).ToReadOnlyReactiveProperty()
         .AddTo(disposables);
    }

    public void Dispose() => disposables.Dispose();
}




public class InventoryViewModel : IDisposable
{
    public List<ItemSlotViewModel> Slots { get; }
    public ReactiveProperty<int?> EquippedIndex { get; }

    private InventoryModel model;
    private CompositeDisposable disposables = new CompositeDisposable();

    public InventoryViewModel(InventoryModel model)
    {
        this.model = model;
        Slots = model.Slots.Select(s => new ItemSlotViewModel(s)).ToList();
        EquippedIndex = new ReactiveProperty<int?>(null).AddTo(disposables);
    }

    public void AddItem(string itemId, int amount) => model.AddItem(itemId, amount);
    public void RemoveAt(int index, int amount = 1) => model.RemoveItem(index, amount);
    public void Equip(int index)
    {
        model.Equip(index);
        EquippedIndex.Value = index;
    }
    public void UnEquip(int index)
    {
        model.UnEquip(index);
        EquippedIndex.Value = null;
    }

    public void Dispose()
    {
        foreach (var s in Slots) s.Dispose();
        EquippedIndex.Dispose();
        disposables.Dispose();
    }
}