using UnityEngine;

public class PortalGun : MonoBehaviour
{
    [Header("Refs")]
    public Camera cam;
    public GameObject portalPrefabA;
    public GameObject portalPrefabB;
    public PortalPair pair;

    [Header("Placement")]
    public float maxDistance = 40f;
    public string surfaceTag = "PortalSurface";
    public float wallOffset = 0.01f;
    public LayerMask placementMask = ~0;

    [Header("Size")]
    public Vector2 portalSize = new Vector2(1f, 2f); // simmular as portal Mesh
    public float edgePadding = 0.05f;

    private Portal portalA;
    private Portal portalB;

    void Awake()
    {
        if (!cam) cam = Camera.main;
        if (!pair) pair = gameObject.AddComponent<PortalPair>();
    }

    void Update()
    {
        if (Input.GetMouseButtonDown(0)) PlacePortal(ref portalA, portalPrefabA);
        if (Input.GetMouseButtonDown(1)) PlacePortal(ref portalB, portalPrefabB);

        pair.portalA = portalA;
        pair.portalB = portalB;
        pair.Link();
    }

    private void PlacePortal(ref Portal slot, GameObject prefab)
    {
        if (!cam || !prefab) return;

        if (Physics.Raycast(cam.transform.position, cam.transform.forward, out var hit, maxDistance, placementMask, QueryTriggerInteraction.Ignore))
        {
            // check tag
            if (!hit.collider.CompareTag(surfaceTag)) return;

            // calculate transform
            Vector3 pos = hit.point + hit.normal * wallOffset;
            Quaternion rot = RotationOnSurface(hit.normal, cam.transform.up);

            // 포탈이 표면에 들어맞는지 박스캐스트로 검증 가능
            // if (!ValidateSpace(pos, rot)) return;

            // place and reuse
            if (slot == null)
            {
                slot = Instantiate(prefab, pos, rot).GetComponent<Portal>();
            }
            else
            {
                slot.transform.SetPositionAndRotation(pos, rot);
            }

            //// ExitPoint가 없다면 기본 세팅 보정(앞으로 살짝)
            //if (slot.exitPoint == null)
            //{
            //    var exit = new GameObject("ExitPoint").transform;
            //    exit.SetParent(slot.transform, false);
            //    exit.localPosition = Vector3.forward * 0.4f;
            //    exit.localRotation = Quaternion.identity;
            //    slot.exitPoint = exit;
            //}
        }
    }

    // 벽의 법선을 정면으로 하되, '위쪽'은 카메라 Up을 표면에 투영해 자연스러운 회전
    private Quaternion RotationOnSurface(Vector3 normal, Vector3 desiredUp)
    {
        // 표면 위쪽 벡터를 구함: desiredUp에서 normal 성분 제거 후 정규화
        Vector3 right = Vector3.Cross(desiredUp, normal);
        if (right.sqrMagnitude < 1e-6f) right = Vector3.Cross(Vector3.up, normal);
        Vector3 upProj = Vector3.Normalize(Vector3.Cross(normal, right));
        return Quaternion.LookRotation(normal, upProj);
    }

    /*
    // (선택) 가장자리 검증 - 포탈 크기만큼 표면 여유 확인
    private bool ValidateSpace(Vector3 pos, Quaternion rot)
    {
        Vector3 half = new Vector3(portalSize.x * 0.5f - edgePadding, portalSize.y * 0.5f - edgePadding, 0.05f);
        // 박스가 표면 쪽으로 아주 얇게 들어가도록 설정
        return !Physics.CheckBox(pos, half, rot, ~0, QueryTriggerInteraction.Ignore);
    }
    */
}
