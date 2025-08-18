using UnityEngine;

public class Portal : MonoBehaviour
{
    [Header("Linked Portal")]
    public Portal target;
    public Transform exitPoint;

    [Header("Options")]
    public float exitOffset = 0.25f;
    public float reenterBlockTime = 0.15f;

    private void Reset()
    {
        Collider col = GetComponent<Collider>();
        col.isTrigger = true;
        Rigidbody rb = GetComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;
        gameObject.layer = LayerMask.NameToLayer("Portal");
    }

    private void OnTriggerEnter(Collider other)
    {
        TryTeleport(other);
    }

    private void TryTeleport(Collider hit)
    {
        // if no exit portal, do nothing
        if (!target) return;

        // check colider has PortalTraveler
        PortalTraveler traveler = hit.GetComponentInParent<PortalTraveler>();
        if (!traveler) return;
        if (traveler.IsBlocked(this)) return;

        Transform travelerTransform = traveler.transform;

        // set transform of object moving trough portal
        Vector3 outPos;         // position
        Quaternion outRot;      // rotation
        outPos = exitPoint.position;
        outRot = exitPoint.rotation;

        //if (exitPoint)
        //{
        //    outPos = exitPoint.position;
        //    outRot = exitPoint.rotation;
        //}
        //else
        //{
        //    //Vector3 localPos = transform.InverseTransformPoint(t.position);
        //    //Quaternion localRot = Quaternion.Inverse(transform.rotation) * t.rotation;
        //    //outPos = exitPortal.transform.TransformPoint(localPos);
        //    //outRot = exitPortal.transform.rotation * localRot;

        //    Debug.LogWarning("There is no exit point of portal");
        //}
            
        // 포탈 약간 앞으로 밀기
        //Vector3 pushDir = (exitPoint ? exitPoint.forward : exitPortal.transform.forward);
        //outPos += pushDir * exitOffset;
        
        // apply to player
        if (travelerTransform.TryGetComponent<Rigidbody>(out var rb))           // 물리 이동형 플레이어
        {
            Vector3 localVel = transform.InverseTransformDirection(rb.velocity);
            Vector3 outVel = target.transform.TransformDirection(localVel);
            rb.position = outPos;
            rb.rotation = outRot;
            rb.velocity = outVel;
        }
        else if (travelerTransform.TryGetComponent<CharacterController>(out var cc)) // CC 이동형
        {
            cc.enabled = false;
            travelerTransform.SetPositionAndRotation(outPos, outRot);
            cc.enabled = true;
        }
        else
        {
            travelerTransform.SetPositionAndRotation(outPos, outRot);
        }

        // block re-enter to portal until cooldown
        traveler.SetCooldown(target, reenterBlockTime);
    }

    // view in editer
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, new Vector3(1f, 2f, 0.05f));
        var forward = exitPoint ? exitPoint.forward : transform.forward;
        var origin = exitPoint ? exitPoint.position : transform.position;
        Gizmos.DrawRay(origin, forward * 0.6f);
    }
}
