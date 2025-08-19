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
    public Dictionary<string, int> Items { get; private set; } = new Dictionary<string, int>();

    public void AddItem(string itemName, int amount = 1)
    {
        if (Items.ContainsKey(itemName)) Items[itemName] += amount;
        else Items[itemName] = amount;
    }

    public void RemoveItem(string itemName, int amount = 1)
    {
        if (Items.ContainsKey(itemName))
        {
            Items[itemName] -= amount;
            if (Items[itemName] <= 0) Items.Remove(itemName);
        }
    }
}