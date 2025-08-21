using System;
using System.Collections;
using System.Collections.Generic;
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
        Inventory = new InventoryModel(100);
    }
}
[Serializable]
public class InventorySlot
{
    public string ItemId;     // 비어있으면 null/empty
    public int Quantity;
    public bool Equipped;
    public bool IsEmpty => string.IsNullOrEmpty(ItemId);
}

public sealed class InventoryModel
{
    public int MaxSlots { get; }
    public List<InventorySlot> Slots { get; }  // index가 곧 '슬롯 인덱스'

    public InventoryModel(int maxSlots)
    {
        MaxSlots = maxSlots;
        Slots = new List<InventorySlot>(maxSlots);
        for (int i = 0; i < maxSlots; i++) Slots.Add(new InventorySlot());
    }

    // stackable 아이템 추가: 기존 스택 채우고 → 빈 슬롯 생성
    // 반환값: 다 못 넣고 남은 개수(0이면 전부 성공)
    public int AddStackable(string itemId, int amount, int maxStack)
    {
        if (amount <= 0) return 0;

        // 1) 기존 스택 채우기
        for (int i = 0; i < Slots.Count && amount > 0; i++)
        {
            var s = Slots[i];
            if (s.ItemId == itemId && s.Quantity < maxStack)
            {
                int add = Math.Min(maxStack - s.Quantity, amount);
                s.Quantity += add;
                amount -= add;
            }
        }
        // 2) 빈 슬롯에 새 스택 생성
        for (int i = 0; i < Slots.Count && amount > 0; i++)
        {
            var s = Slots[i];
            if (s.IsEmpty)
            {
                int add = Math.Min(maxStack, amount);
                s.ItemId = itemId;
                s.Quantity = add;
                amount -= add;
            }
        }
        return amount;
    }

    public bool RemoveAt(int slotIndex, int amount = 1)
    {
        if (slotIndex < 0 || slotIndex >= Slots.Count) return false;
        var s = Slots[slotIndex];
        if (s.IsEmpty || amount <= 0) return false;

        s.Quantity -= amount;
        if (s.Quantity <= 0)
        {
            s.ItemId = null;
            s.Quantity = 0;
            s.Equipped = false;
        }
        return true;
    }

    public void Equip(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= Slots.Count) return;
        if (Slots[slotIndex].IsEmpty) return;

        // 단일 장착형 예시: 전부 해제 후 해당 슬롯만 장착
        for (int i = 0; i < Slots.Count; i++) Slots[i].Equipped = false;
        Slots[slotIndex].Equipped = true;
    }

    public void UnEquip(int slotIndex)
    {
        if (slotIndex < 0 || slotIndex >= Slots.Count) return;
        Slots[slotIndex].Equipped = false;
    }
}