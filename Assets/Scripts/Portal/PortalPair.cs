using UnityEngine;

public class PortalPair : MonoBehaviour
{
    public Portal portalA;
    public Portal portalB;

    public void Link()
    {
        if (portalA && portalB)
        {
            portalA.target = portalB;
            portalB.target = portalA;
        }
    }
}
