using UnityEngine;

public abstract class Tool : MonoBehaviour
{
    public string displayName = "Tool";
    public float primaryCooldown, secondaryCooldown;
    protected float primaryReadyTime, secondaryReadyTime;

    [HideInInspector] public Camera useCamera;
    [HideInInspector] public Transform owner;

    public virtual void OnEquip(Transform newOwner, Camera cam, ToolItemData data = null)
    {
        owner = newOwner; useCamera = cam; gameObject.SetActive(true);
        if (data) { displayName = data.displayName; primaryCooldown = data.primaryCooldown; secondaryCooldown = data.secondaryCooldown; }
    }
    public virtual void OnUnequip() { gameObject.SetActive(false); owner = null; useCamera = null; }

    public void TryUsePrimary() { if (Time.time >= primaryReadyTime && UsePrimary()) primaryReadyTime = Time.time + primaryCooldown; }
    public void TryUseSecondary() { if (Time.time >= secondaryReadyTime && UseSecondary()) secondaryReadyTime = Time.time + secondaryCooldown; }

    protected abstract bool UsePrimary();
    protected abstract bool UseSecondary();
}
