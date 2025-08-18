using UnityEngine;

public enum ItemType { Misc, Consumable, Tool }

public abstract class ItemData : ScriptableObject
{
    [Header("Identity")]
    public string id;                 // for save
    public string displayName;
    public ItemType type = ItemType.Misc;
    public Sprite icon;
    public bool stackable = true;
    public int maxStack = 99;

    [ContextMenu("Generate New ID")]
    void GenerateId()
    { 
        id = System.Guid.NewGuid().ToString("N");
    }
}
