using System;
using System.Collections;
using System.Collections.Generic;
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
    public ReactiveProperty<string> ItemId { get; }
    public ReactiveProperty<int> Quantity { get; }
    public ReactiveProperty<bool> Equipped { get; }

    public ReadOnlyReactiveProperty<bool> IsEmpty { get; }

    public InventorySlot(string itemId = null, int quantity = 0, bool equipped = false)
    {
        ItemId = new ReactiveProperty<string>(itemId);
        Quantity = new ReactiveProperty<int>(quantity);
        Equipped = new ReactiveProperty<bool>(equipped);

        IsEmpty = ItemId.Select(id => string.IsNullOrEmpty(id)).ToReadOnlyReactiveProperty();
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
    public void AddItem(string itemId, int amount)
    {
        foreach (var slot in Slots)
        {
            if (slot.IsEmpty.Value)
            {
                slot.ItemId.Value = itemId;
                slot.Quantity.Value = amount;
                break;
            }
        }
    }
    public void RemoveItem(int index, int amount)
    {
        var slot = Slots[index];
        slot.Quantity.Value -= amount;
        if (slot.Quantity.Value <= 0)
        {
            slot.ItemId.Value = null;
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