using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Collider))]
public class Portal : MonoBehaviour
{
    [Header("Linked Portal")]
    public Portal target;
    public Transform exitPoint; // use target.transform if null

    [Header("Options")]
    [Tooltip("�ⱸ���� ��¦ ������ �о �Ÿ�")]
    public float exitOffset = 0.5f;

    [Tooltip("������ ���� �ð�(��)")]
    public float reenterBlockTime = 0.15f;

    [Tooltip("��Ż �ո鿡���� ���� ���")]
    public bool requireEntryFromFront = true; // ���� ��� 

    private void Reset()
    {
        // Collider�� �ʼ�(RequireComponent�� ����), Ʈ���� ����
        var col = GetComponent<Collider>();
        col.isTrigger = true;

        // CC�� Ʈ���� ��ȣ�ۿ��� �����Ϸ��� ��Ż �ʿ� Kinematic Rigidbody �ʿ�
        var rb = GetComponent<Rigidbody>();
        if (rb == null) rb = gameObject.AddComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;

        // ���̾ ������ ���� ����(������ -1)
        int portalLayer = LayerMask.NameToLayer("Portal");
        if (portalLayer != -1) gameObject.layer = portalLayer;
    }

    private void OnTriggerEnter(Collider other) => TryTeleport(other);
    
    private void OnTriggerStay(Collider other) => TryTeleport(other);

    private void TryTeleport(Collider hit)
    {
        if (!target) return;

        // ������ Ȯ��
        var traveler = hit.GetComponentInParent<PortalTraveler>();
        if (!traveler) return;
        if (traveler.IsBlocked(this)) return;

        // �޸� ���� ����
        if (requireEntryFromFront)
        {
            Vector3 closest = hit.ClosestPoint(transform.position);
            Vector3 toObj = closest - transform.position;
            if (Vector3.Dot(transform.forward, toObj) < 0f) return;
        }

        var travelerTransform = traveler.transform;

        // ���� ������ ����
        Transform fromFrame = transform;
        Transform toFrame = (target.exitPoint != null) ? target.exitPoint : target.transform;

        // �����ǥ ��� ��ġ/ȸ�� ��ȯ
        Vector3 localPos = fromFrame.InverseTransformPoint(travelerTransform.position);
        Quaternion localRot = Quaternion.Inverse(fromFrame.rotation) * travelerTransform.rotation;

        Vector3 outPos = toFrame.TransformPoint(localPos);
        Quaternion outRot = toFrame.rotation * localRot;

        // �ⱸ �������� �̼� ������(Ʈ���� ���浹 ����)
        outPos += toFrame.forward * exitOffset;

        // ����
        if (travelerTransform.TryGetComponent<Rigidbody>(out var rb)) // ���� �̵���
        {
            Vector3 localVel = fromFrame.InverseTransformDirection(rb.velocity);
            Vector3 outVel = toFrame.TransformDirection(localVel);

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
        else if (!travelerTransform.TryGetComponent<Rigidbody>(out _)) // RB/CC �� �� �ƴϸ� ���� ����
        {
            travelerTransform.SetPositionAndRotation(outPos, outRot);
        }

        // �÷��̾� ����� ������
        if (travelerTransform.TryGetComponent<PlayerController>(out var pc))
            pc.OnTeleported(fromFrame, toFrame);

        // ������ ��ٿ�(���� ��� ������: ������ ��ȭ)
        traveler.SetCooldown(target, reenterBlockTime);
        traveler.SetCooldown(this, reenterBlockTime);
    }

    // view in editor
    private void OnDrawGizmos()
    {
        Gizmos.color = Color.cyan;
        Gizmos.DrawWireCube(transform.position, new Vector3(1f, 2f, 0.05f));

        Transform toFrame = exitPoint ? exitPoint : transform;
        var origin = toFrame.position;
        var dir = toFrame.forward;
        Gizmos.DrawRay(origin, -dir * 0.3f);
    }
}
