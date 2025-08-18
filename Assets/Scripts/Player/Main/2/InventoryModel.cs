using System.Collections;
using System.Collections.Generic;
using UnityEngine;


#region --- PlayerViewModel + InventoryModel ---

#endregion


public class InventoryModel1 : MonoBehaviour
{
    public Dictionary<string, int> Items { get; private set; } = new Dictionary<string, int>();

    public void AddItem(string itemName, int amount = 1)
    {
        if (Items.ContainsKey(itemName)) Items[itemName] += amount;
        else Items[itemName] = amount;
    }

    public void RemoveItem(string itemName, int amount = 1)
    {
        if (!Items.ContainsKey(itemName)) return;

        Items[itemName] -= amount;
        if (Items[itemName] <= 0) Items.Remove(itemName);
    }

}
