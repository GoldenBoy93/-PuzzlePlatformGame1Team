using UnityEngine;

public class ItemSlot : MonoBehaviour
{
    public Camera playerCamera;
    public Transform mount;
    public Tool equipped;

    void Awake()
    {
        if (!playerCamera) playerCamera = Camera.main;
        if (!mount) mount = transform;
    }

    public bool EquipByItemId(string itemId)
    {
        var data = ItemDatabase.Instance.GetById(itemId) as ToolItemData;
        if (!data || !data.toolPrefab) return false;

        Unequip();

        equipped = Instantiate(data.toolPrefab, mount);
        equipped.OnEquip(transform, playerCamera, data);
        return true;
    }

    public void Unequip()
    {
        if (!equipped) return;
        equipped.OnUnequip();
        Destroy(equipped.gameObject);
        equipped = null;
    }

    // 입력 바인딩용
    public void UsePrimary() => equipped?.TryUsePrimary();
    public void UseSecondary() => equipped?.TryUseSecondary();
}
