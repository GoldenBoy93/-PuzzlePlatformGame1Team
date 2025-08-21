using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UniRx;
using UnityEngine;

public sealed class PlayerModel
{
    public int MaxHealth { get; }
    public int MaxStamina { get; }
    private int health;
    public int Health { get => health; set => health = Mathf.Clamp(value, 0, MaxHealth); }
    private int stamina;
    public int Stamina { get => stamina; set => stamina = Mathf.Clamp(value, 0, MaxStamina); }
    public InventoryModel Inventory { get; }
    public PlayerModel(int maxHealth = 5, int maxStamina = 1000)
    {
        MaxHealth = maxHealth;
        MaxStamina = maxStamina;
        health = MaxHealth;
        stamina = MaxStamina;
        Inventory = new InventoryModel(24);
    }
}

[Serializable]
public class InventorySlot
{
    public ReactiveProperty<ItemData> Item { get; }
    public ReactiveProperty<int> Quantity { get; }
    public ReactiveProperty<bool> Equipped { get; }

    public ReadOnlyReactiveProperty<bool> IsEmpty { get; }

    public InventorySlot(ItemData item = null, int quantity = 0, bool equipped = false)
    {
        Item = new ReactiveProperty<ItemData>(item);
        Quantity = new ReactiveProperty<int>(quantity);
        Equipped = new ReactiveProperty<bool>(equipped);

        IsEmpty = Item.Select(i => i == null).ToReadOnlyReactiveProperty();
    }
}

public sealed class InventoryModel
{
    public int MaxSlots { get; }
    public List<InventorySlot> Slots { get; }
    public InventoryModel(int maxSlots)
    {
        MaxSlots = maxSlots;
        Slots = new List<InventorySlot>();
        for (int i = 0; i < maxSlots; i++)
            Slots.Add(new InventorySlot());
    }
    public void AddItem(ItemData item, int amount)
    {
        if (item == null) return;

        // 스택 가능한 슬롯 찾기
        if (item.stackable)
        {
            var slot = Slots.FirstOrDefault(s => s.Item.Value == item && s.Quantity.Value < item.maxStack);
            if (slot != null)
            {
                slot.Quantity.Value = Mathf.Min(slot.Quantity.Value + amount, item.maxStack);
                return;
            }
        }

        // 빈 슬롯 찾아서 새 아이템 넣기
        var emptySlot = Slots.FirstOrDefault(s => s.Item.Value == null);
        if (emptySlot != null)
        {
            emptySlot.Item.Value = item;
            emptySlot.Quantity.Value = Mathf.Min(amount, item.maxStack);
        }
    }




    public void RemoveItem(int index, int amount)
    {
        var slot = Slots[index];
        slot.Quantity.Value -= amount;
        if (slot.Quantity.Value <= 0)
        {
            slot.Item.Value = null;
            slot.Quantity.Value = 0;
            slot.Equipped.Value = false;
        }
    }
    public void Equip(int index)
    {
        for (int i = 0; i < Slots.Count; i++)
            Slots[i].Equipped.Value = false;

        Slots[index].Equipped.Value = true;
    }
    public void UnEquip(int index)
    {
        Slots[index].Equipped.Value = false;
    }
}