using UnityEngine;

public class ItemPickup : MonoBehaviour
{
    public ItemData data;
    public int amount = 1;

    private void OnTriggerEnter(Collider other)
    {
        var inv = other.GetComponentInChildren<InventoryTemp>();
        if (!inv || !data) return;

        if (inv.Add(data.id, amount))
            Destroy(gameObject);
    }
}
