using UnityEngine;

[DisallowMultipleComponent]
[RequireComponent(typeof(Collider))]
public class Portal : MonoBehaviour
{
    [Header("Linked Portal")]
    public Portal target;
    public Transform exitPoint; // use target.transform if null

    [Header("Options")]
    [Tooltip("출구에서 살짝 앞으로 밀어낼 거리")]
    public float exitOffset = 0.5f;

    [Tooltip("재진입 방지 시간(초)")]
    public float reenterBlockTime = 0.15f;

    [Tooltip("포탈 앞면에서만 진입 허용")]
    public bool requireEntryFromFront = true; // 선택 기능 

    private void Reset()
    {
        // Collider는 필수(RequireComponent로 보장), 트리거 설정
        var col = GetComponent<Collider>();
        col.isTrigger = true;

        // CC와 트리거 상호작용을 보장하려면 포탈 쪽에 Kinematic Rigidbody 필요
        var rb = GetComponent<Rigidbody>();
        if (rb == null) rb = gameObject.AddComponent<Rigidbody>();
        rb.isKinematic = true;
        rb.useGravity = false;

        // 레이어가 존재할 때만 설정(없으면 -1)
        int portalLayer = LayerMask.NameToLayer("Portal");
        if (portalLayer != -1) gameObject.layer = portalLayer;
    }

    private void OnTriggerEnter(Collider other) => TryTeleport(other);
    
    private void OnTriggerStay(Collider other) => TryTeleport(other);

    private void TryTeleport(Collider hit)
    {
        if (!target) return;

        // 여행자 확인
        var traveler = hit.GetComponentInParent<PortalTraveler>();
        if (!traveler) return;
        if (traveler.IsBlocked(this)) return;

        // 뒷면 진입 차단
        if (requireEntryFromFront)
        {
            Vector3 closest = hit.ClosestPoint(transform.position);
            Vector3 toObj = closest - transform.position;
            if (Vector3.Dot(transform.forward, toObj) < 0f) return;
        }

        var travelerTransform = traveler.transform;

        // 기준 프레임 정의
        Transform fromFrame = transform;
        Transform toFrame = (target.exitPoint != null) ? target.exitPoint : target.transform;

        // 상대좌표 기반 위치/회전 변환
        Vector3 localPos = fromFrame.InverseTransformPoint(travelerTransform.position);
        Quaternion localRot = Quaternion.Inverse(fromFrame.rotation) * travelerTransform.rotation;

        Vector3 outPos = toFrame.TransformPoint(localPos);
        Quaternion outRot = toFrame.rotation * localRot;

        // 출구 방향으로 미세 오프셋(트리거 재충돌 방지)
        outPos += toFrame.forward * exitOffset;

        // 적용
        if (travelerTransform.TryGetComponent<Rigidbody>(out var rb)) // 물리 이동형
        {
            Vector3 localVel = fromFrame.InverseTransformDirection(rb.velocity);
            Vector3 outVel = toFrame.TransformDirection(localVel);

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
        else if (!travelerTransform.TryGetComponent<Rigidbody>(out _)) // RB/CC 둘 다 아니면 직접 세팅
        {
            travelerTransform.SetPositionAndRotation(outPos, outRot);
        }

        // 플레이어 모멘텀 유지용
        if (travelerTransform.TryGetComponent<PlayerController>(out var pc))
            pc.OnTeleported(fromFrame, toFrame);

        // 재진입 쿨다운(양쪽 모두 스탬프: 안전망 강화)
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
