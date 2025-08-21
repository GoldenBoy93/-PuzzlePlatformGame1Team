using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerModel //�ܼ������� ����
{
    public int Health { get; set; } = 5;
    public int MaxHealth { get; set; } = 5;
    public int Stamina { get; set; } = 1000;
    public int MaxStamina { get; set; } = 1000;


    // �κ��丮 ���� ���� (Composition)
    public InventoryModel Inventory { get; private set; }

    public PlayerModel()
    {
        Inventory = new InventoryModel(); // ���� �� ���� �ʱ�ȭ
    }
}

public class InventoryModel
{
    // �ܼ� ������ ���� (�̸� + ����)
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