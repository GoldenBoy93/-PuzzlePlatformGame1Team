using UnityEngine;

[CreateAssetMenu(fileName = "Consumable_", menuName = "Items/Consumable Item", order = 1)]
public class ConsumableItemData : ItemData
{
    [Header("Consumable")]
    public float healAmount = 1f;

    protected override void OnValidate()
    {
        base.OnValidate();

        type = ItemType.Consumable;
    }
}