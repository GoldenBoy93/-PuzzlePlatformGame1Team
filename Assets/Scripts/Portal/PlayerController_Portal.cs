using UnityEngine;

public partial class PlayerController : MonoBehaviour
{
    [Header("Portal")]
    [SerializeField] bool yawOnlyOnTeleport = true;    // ���ϰ� ����, �¿�(Yaw)�� ����
    [SerializeField] bool skipGravityOneFrame = true;  // �ڷ���Ʈ ������ �߷� ��ŵ

    // ���� �÷��� (�߷� 1������ ��ŵ��)
    bool _skipGravityThisFrame;

    [Header("Portal (Placement)")]
    [SerializeField] PortalManager portalManager;             // �ν����� �Ҵ�
    [SerializeField] GameObject crosshair;                    // Ctrl�� ǥ��/��ǥ��
    [SerializeField] LayerMask portalSurfaceMask;             // ��ġ ���� ǥ��
    [SerializeField] LayerMask portalObstructMask;            // ��ġ ���� ���� ���̾�
    [SerializeField] float maxPlaceDistance = 40f;            // ���� �Ÿ�
    [SerializeField] float portalDepthOffset = 0.01f;         // ǥ�� �� �İ���� �ʰ�
    [SerializeField] Vector3 portalHalfExtents = new(0.5f, 1.0f, 0.05f); // ��Ż �뷫 ũ��

    // ���� ��� (Ctrl�� ���)
    bool _portalMode;

    [SerializeField] float minExitUpSpeed = 3f;         // �ٴ� ��Ż���� �ּ� ��� �ӵ�
    [SerializeField, Range(0f, 1f)] float floorDot = 0.5f; // �ⱸ�� '����'�� ���Ѵٰ� �� �Ӱ谪(�ڻ���)

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
    private void UpdatePortal() { /* �ʿ� �� ���*/ }
    
    // Portal.cs�� ȣ��
    public void OnTeleported(Transform from, Transform to)
    {
        // ȸ�� ����
        if (yawOnlyOnTeleport)
        {
            Quaternion delta = to.rotation * Quaternion.Inverse(from.rotation);
            float newYaw = (delta * transform.rotation).eulerAngles.y;
            transform.rotation = Quaternion.Euler(0f, newYaw, 0f);
        }
        // ��ü ȸ�� ������ �ʿ��ϸ� �� ����� ����, Portal.cs�� ������ outRot�� �״�� ����ϸ� ��.

        // �߷� 1������ ��ŵ (�� �������� ���� ����)
        if (skipGravityOneFrame) _skipGravityThisFrame = true;

        // �ⱸ ���� ű (CharacterController��)
        float d = Vector3.Dot(to.forward, Vector3.up);
        if (d >= floorDot)
        {
            // �ٴ� ��Ż: ���� �ּ� �ӵ� Ȯ��
            if (velocity.y < minExitUpSpeed) velocity.y = minExitUpSpeed;
        }
        else if (d <= -floorDot)
        {
            // õ�� ��Ż: �Ʒ��� �ּ� �ӵ� Ȯ�� (���ϸ� ���)
            if (velocity.y > -minExitUpSpeed) velocity.y = -minExitUpSpeed;
        }

        // ���� ���Ŀ� ���� �ӵ��� ���� �����Ѵٸ�, �Ʒ�ó�� ȸ������ �ּ���:
        // velocity = to.TransformDirection(from.InverseTransformDirection(velocity));
    }

    // ��Ŭ�� = A(�Ķ�), ��Ŭ�� = B(����)
    void PlacePortal(bool isA)
    {
        if (!_portalMode || portalManager == null || _camera == null) return;

        Ray ray = _camera.ScreenPointToRay(Input.mousePosition);
        if (!Physics.Raycast(ray, out var hit, maxPlaceDistance, portalSurfaceMask, QueryTriggerInteraction.Ignore))
            return;

        // ��ġ ���� ǥ�� Ȯ��(�±�)
        if (!hit.collider.CompareTag("PortalSurface"))
            return;

        // ǥ�� ������ ���� ����
        Vector3 pos = hit.point + hit.normal * portalDepthOffset;
        Quaternion rot = Quaternion.LookRotation(-hit.normal, Vector3.up);
        rot *= Quaternion.Euler(0f, 180f, 0f);  // �����տ� �°� y�� �������� ������

        // ���� ���� üũ(��ġ�� ���)
        if (Physics.CheckBox(pos, portalHalfExtents, rot, portalObstructMask, QueryTriggerInteraction.Ignore))
            return;

        if (isA) portalManager.PlaceA(pos, rot);  // �Ķ�
        else portalManager.PlaceB(pos, rot);  // ����
    }
}
