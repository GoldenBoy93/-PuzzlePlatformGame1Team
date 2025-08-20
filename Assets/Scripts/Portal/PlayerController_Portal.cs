using System;
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
    //[SerializeField] LayerMask portalSurfaceMask;             // ��ġ ���� ǥ��
    //[SerializeField] LayerMask portalObstructMask;            // ��ġ ���� ���� ���̾�
    [SerializeField] float maxPlaceDistance = 40f;            // ���� �Ÿ�
    [SerializeField] float portalDepthOffset = 0.01f;         // ǥ�� �� �İ���� �ʰ�
    //[SerializeField] Vector3 portalHalfExtents = new(0.5f, 1.0f, 0.05f); // ��Ż �뷫 ũ��

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
        // Debug.Log("��Ż ��ġ �޼��� ����");

        if (!_portalMode || portalManager == null || _camera == null)
        {

            Debug.Log("��Ż ��尡 �ƴϰų� �Ҵ��� �ȵǾ� ����");
            return;
        }

        // ȭ�� �߾�(���ؼ�) ����
        Ray ray = GetRayFromCrosshair(_camera, crosshair);

        // Portal ���̾� ���� ���� ���, �浹ü ���� ����
        int portalLayer = LayerMask.NameToLayer("Portal");
        int mask = (portalLayer >= 0) ? ~(1 << portalLayer) : Physics.DefaultRaycastLayers;

        // RaycastAll�� ���� ���� ��, ���� ����� ��ȿ ��Ʈ�� ����
        var hits = Physics.RaycastAll(ray, maxPlaceDistance, mask, QueryTriggerInteraction.Collide);
        if (hits.Length == 0) return;       // �浹 ���� �� ����
        Array.Sort(hits, (a, b) => a.distance.CompareTo(b.distance));

        // ��ȿ �±� ã�� - ������ �޼��� ����
        RaycastHit hit = default;
        bool found = false;
        foreach (var h in hits)
        {
            // �� �÷��̾� �ݶ��̴��� �ǳʶٱ�
            if (h.collider.GetComponentInParent<PlayerController>() == this) continue;

            // �θ���� �ö󰡸� PortalSurface �±� Ȯ��
            if (!HasTagInParents(h.collider.transform, "PortalSurface")) continue;

            hit = h;
            found = true;
            break;
        }

        if (!found)
        {
            // Debug.Log("[Portal] No valid surface in ray path (player or no PortalSurface tag).");
            return;
        }

        /*
        // ���� �ݶ��̴� ���� �����
        var col = hit.collider;
        Debug.Log($"[Portal] Hit='{col.name}', tag='{col.tag}', layer='{LayerMask.LayerToName(col.gameObject.layer)}', path='{GetTransformPath(col.transform)}'");

        // ��ġ ���� ǥ�� Ȯ��(�±�)
        Transform taggedRoot = FindTagInParents(col.transform, "PortalSurface");
        if (taggedRoot == null)
        {
            Debug.Log("[Portal] �θ���� 'PortalSurface]' �ױװ� ����");
            return;
        }
        */

        // ǥ�� ������ ���� ����
        Vector3 pos = hit.point + hit.normal * portalDepthOffset;
        Quaternion rot = Quaternion.LookRotation(-hit.normal, Vector3.up);
        rot *= Quaternion.Euler(0f, 180f, 0f);  // �����տ� �°� y�� �������� ������

        /*
        // ���� ���� üũ(��ġ�� ���)
        if (Physics.CheckBox(pos, portalHalfExtents, rot, portalObstructMask, QueryTriggerInteraction.Ignore))
            return;
        */

        if (isA) portalManager.PlaceA(pos, rot);  // �Ķ�
        else portalManager.PlaceB(pos, rot);  // ����
    }

    Ray GetRayFromCrosshair(Camera cam, GameObject crosshair)
    {
        if (crosshair == null)
            return cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        var rt = crosshair.GetComponent<RectTransform>();
        if (rt == null)
            return cam.ViewportPointToRay(new Vector3(0.5f, 0.5f, 0f));

        var canvas = rt.GetComponentInParent<Canvas>();
        Vector3 screenPos;

        // Screen Space - Overlay �� ����潺ũ�� ��ȯ ���ʿ�
        if (canvas && canvas.renderMode == RenderMode.ScreenSpaceOverlay)
            screenPos = rt.position;
        else
            screenPos = RectTransformUtility.WorldToScreenPoint(cam, rt.position);

        return cam.ScreenPointToRay(screenPos);
    }

    bool HasTagInParents(Transform t, string tagName)
    {
        while (t != null)
        {
            if (t.CompareTag(tagName)) return true;
            t = t.parent;
        }
        return false;
    }
}
