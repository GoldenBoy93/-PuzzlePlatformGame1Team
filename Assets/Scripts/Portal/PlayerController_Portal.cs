using UnityEngine;

public partial class PlayerController : MonoBehaviour
{
    [Header("Portal")]
    [SerializeField] bool yawOnlyOnTeleport = true;    // 상하각 고정, 좌우(Yaw)만 맞춤
    [SerializeField] bool skipGravityOneFrame = true;  // 텔레포트 프레임 중력 스킵

    // 내부 플래그 (중력 1프레임 스킵용)
    bool _skipGravityThisFrame;

    [Header("Portal (Placement)")]
    [SerializeField] PortalManager portalManager;             // 인스펙터 할당
    [SerializeField] GameObject crosshair;                    // Ctrl로 표시/비표시
    [SerializeField] LayerMask portalSurfaceMask;             // 설치 가능 표면
    [SerializeField] LayerMask portalObstructMask;            // 설치 공간 방해 레이어
    [SerializeField] float maxPlaceDistance = 40f;            // 레이 거리
    [SerializeField] float portalDepthOffset = 0.01f;         // 표면 안 파고들지 않게
    [SerializeField] Vector3 portalHalfExtents = new(0.5f, 1.0f, 0.05f); // 포탈 대략 크기

    // 조준 모드 (Ctrl로 토글)
    bool _portalMode;

    [SerializeField] float minExitUpSpeed = 3f;         // 바닥 포탈에서 최소 상승 속도
    [SerializeField, Range(0f, 1f)] float floorDot = 0.5f; // 출구가 '위쪽'을 향한다고 볼 임계값(코사인)

    // hooks
    private void OnEnablePortal() 
    { 
        _portalMode = false;
        if (crosshair) crosshair.SetActive(false);
    }
    private void OnDisablePortal() 
    {
        _portalMode = false;
        if (crosshair) crosshair.SetActive(false);
        _skipGravityThisFrame = false; 
    }
    private void UpdatePortal() { /* 필요 시 사용*/ }
    
    // Portal.cs가 호출
    public void OnTeleported(Transform from, Transform to)
    {
        // 회전 보정
        if (yawOnlyOnTeleport)
        {
            Quaternion delta = to.rotation * Quaternion.Inverse(from.rotation);
            float newYaw = (delta * transform.rotation).eulerAngles.y;
            transform.rotation = Quaternion.Euler(0f, newYaw, 0f);
        }
        // 전체 회전 보존이 필요하면 위 블록을 끄고, Portal.cs가 세팅한 outRot을 그대로 사용하면 됨.

        // 중력 1프레임 스킵 (툭 떨어지는 느낌 방지)
        if (skipGravityOneFrame) _skipGravityThisFrame = true;

        // 출구 방향 킥 (CharacterController용)
        float d = Vector3.Dot(to.forward, Vector3.up);
        if (d >= floorDot)
        {
            // 바닥 포탈: 위로 최소 속도 확보
            if (velocity.y < minExitUpSpeed) velocity.y = minExitUpSpeed;
        }
        else if (d <= -floorDot)
        {
            // 천장 포탈: 아래로 최소 속도 확보 (원하면 사용)
            if (velocity.y > -minExitUpSpeed) velocity.y = -minExitUpSpeed;
        }

        // 만약 이후에 수평 속도를 직접 관리한다면, 아래처럼 회전시켜 주세요:
        // velocity = to.TransformDirection(from.InverseTransformDirection(velocity));
    }

    // 우클릭 = A(파랑), 좌클릭 = B(빨강)
    void PlacePortal(bool isA)
    {
        if (!_portalMode || portalManager == null || _camera == null) return;

        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out var hit, maxPlaceDistance, portalSurfaceMask, QueryTriggerInteraction.Ignore))
            return;

        // 설치 가능 표면 확인(태그)
        if (!hit.collider.CompareTag("PortalSurface"))
            return;

        // 표면 법선에 수직 정렬
        Vector3 pos = hit.point + hit.normal * portalDepthOffset;
        Quaternion rot = Quaternion.LookRotation(-hit.normal, Vector3.up);
        rot *= Quaternion.Euler(0f, 180f, 0f);  // 프리팹에 맞게 y축 기준으로 뒤집기

        // 공간 여유 체크(겹치면 취소)
        if (Physics.CheckBox(pos, portalHalfExtents, rot, portalObstructMask, QueryTriggerInteraction.Ignore))
            return;

        if (isA) portalManager.PlaceA(pos, rot);  // 파랑
        else portalManager.PlaceB(pos, rot);  // 빨강
    }
}
