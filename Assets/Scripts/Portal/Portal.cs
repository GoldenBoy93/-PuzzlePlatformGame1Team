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

        // push forward
        Vector3 pushDir = exitPoint ? exitPoint.right : target.transform.right;
        outPos += pushDir * exitOffset;

        // apply to player
        if (travelerTransform.TryGetComponent<Rigidbody>(out var rb))           // ���� �̵��� �÷��̾�
        {
            Vector3 localVel = transform.InverseTransformDirection(rb.velocity);
            Vector3 outVel = (exitPoint ? exitPoint : target.transform).TransformDirection(localVel);
            rb.position = outPos;
            rb.rotation = outRot;
            rb.velocity = outVel;
        }
        else if (travelerTransform.TryGetComponent<CharacterController>(out var cc)) // CC �̵���
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
        var dir = exitPoint ? exitPoint.right : transform.right;
        var origin = exitPoint ? exitPoint.position : transform.position;
        Gizmos.DrawRay(origin, dir * 0.6f);
    }
}
