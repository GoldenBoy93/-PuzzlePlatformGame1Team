using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

// ������ ReactiveProperty�� ���δ� ViewModel
public class ItemSlotViewModel : IDisposable
{
    public InventorySlot Slot { get; }
    public ReadOnlyReactiveProperty<string> LabelText { get; }
    public ReadOnlyReactiveProperty<Sprite> Icon { get; }

    private CompositeDisposable disposables = new CompositeDisposable();

    public ItemSlotViewModel(InventorySlot slot)
    {
        Slot = slot;

LabelText = Observable.CombineLatest(
    Slot.Item, Slot.Quantity, Slot.Equipped,
    (item, qty, eq) =>
    {
        if (item == null) return "";   // �� �����̸� �� ���ڿ�

        // ItemData ���� displayName ���
        return $"{item.displayName} x{qty}" + (eq ? " [E]" : "");
    })
    .ToReadOnlyReactiveProperty()
    .AddTo(disposables);

// �����ܵ� ItemData���� �ٷ� ��������
Icon = Slot.Item
    .Select(item => item?.icon)
    .ToReadOnlyReactiveProperty()
    .AddTo(disposables);    }

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
    public void AddItem(ItemData item, int amount)
    {
        model.AddItem(item, amount);
        CheckKeyCondition();

    }
    public void RemoveAt(int index, int amount = 1) 
        => model.RemoveItem(index, amount);
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

    public void CheckKeyCondition()
    {
        // ItemType�� Key�� ������ ���� ����
        int keyCount = Slots
            .Where(s => s.Slot.Item.Value != null && s.Slot.Item.Value.type == ItemType.key)
            .Sum(s => s.Slot.Quantity.Value);

        if (keyCount >= 3)
        {
            OnThreeKeysCollected?.Invoke(); // �̺�Ʈ �߻�
        }
    }

    // �̺�Ʈ ����
    public event Action OnThreeKeysCollected;
}