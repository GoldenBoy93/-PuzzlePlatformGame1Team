using System;
using System.Collections.Generic;
using System.IO;
using UnityEngine;

[Serializable]
public class ItemStack
{
    public string itemId;
    public int quantity;

    public ItemStack() { }
    public ItemStack(string id, int qty) { itemId = id; quantity = qty; }
}

public class InventoryTemp : MonoBehaviour
{
    public List<ItemStack> stacks = new();
    public Action OnChanged;

    string SavePath => Path.Combine(Application.persistentDataPath, "inv_temp.json");

    public bool Add(string itemId, int qty)
    {
        var data = ItemManager.Instance.GetById(itemId);
        if (data == null) return false;

        if (data.stackable)
        {
            var s = stacks.Find(x => x.itemId == itemId);
            if (s != null) s.quantity = Mathf.Clamp(s.quantity + qty, 0, data.maxStack);
            else stacks.Add(new ItemStack(itemId, Mathf.Clamp(qty, 1, data.maxStack)));
        }
        else
        {
            // 비스택형은 개수만큼 단독 스택 생성 (임시 인벤토리라면 1로 제한 권장)
            if (qty > 0) stacks.Add(new ItemStack(itemId, 1));
        }
        OnChanged?.Invoke();
        return true;
    }

    public bool Remove(string itemId, int qty)
    {
        var s = stacks.Find(x => x.itemId == itemId);
        if (s == null) return false;
        s.quantity -= qty;
        if (s.quantity <= 0) stacks.Remove(s);
        OnChanged?.Invoke();
        return true;
    }

    [Serializable] class SaveBlob { public List<ItemStack> stacks; }

    public void Save()
    {
        var blob = new SaveBlob { stacks = stacks };
        File.WriteAllText(SavePath, JsonUtility.ToJson(blob, true));
    }

    public void Load()
    {
        if (!File.Exists(SavePath)) return;
        var json = File.ReadAllText(SavePath);
        var blob = JsonUtility.FromJson<SaveBlob>(json);
        stacks = blob?.stacks ?? new List<ItemStack>();
        OnChanged?.Invoke();
    }
}
