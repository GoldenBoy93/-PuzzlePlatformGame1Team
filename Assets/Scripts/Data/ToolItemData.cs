using UnityEngine;

[CreateAssetMenu(menuName = "Items/Tool Data")]
public class ToolItemData : ItemData
{
    [Header("Tool Prefab")]
    public Tool toolPrefab;               // 반드시 Tool 상속 프리팹
    [Header("Cooldowns")]
    public float primaryCooldown = 0f;
    public float secondaryCooldown = 0f;

    private void OnValidate() { type = ItemType.Tool; stackable = false; maxStack = 1; }
}
