using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel //단순데이터 저장
{
    public int Health { get; set; } = 5;
    public int MaxHealth { get; set; } = 5;
    public int Stamina { get; set; } = 1000;
    public int MaxStamina { get; set; } = 1000;


    // 인벤토리 모델을 참조 (Composition)
    public InventoryModel Inventory { get; private set; }

    public PlayerModel()
    {
        Inventory = new InventoryModel(); // 생성 시 같이 초기화
    }
}

public class InventoryModel
{
    // 단순 아이템 저장 (이름 + 수량)
    public Dictionary<ItemData, int> Items { get; private set; } = new Dictionary<ItemData, int>();

    public void AddItem(ItemData data, int amount = 1)
    {
        if (Items.ContainsKey(data)) Items[data] += amount;
        else Items[data] = amount;
    }

    public void RemoveItem(ItemData data, int amount = 1)
    {
        if (Items.ContainsKey(data))
        {
            Items[data] -= amount;
            if (Items[data] <= 0) Items.Remove(data);
        }
    }
}