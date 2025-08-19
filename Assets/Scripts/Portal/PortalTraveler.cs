using UnityEngine;

public class PortalTraveler : MonoBehaviour
{
    private Portal lastPortal;
    private float cooldownUntil;

    public bool IsBlocked(Portal p)
    {
        if ( (p == lastPortal) && Time.time < cooldownUntil )
            return true;
        else 
            return false;
    }

    public void SetCooldown(Portal p, float seconds)
    {
        lastPortal = p;
        cooldownUntil = Time.time + seconds;
    }
}
