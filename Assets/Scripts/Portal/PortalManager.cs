using UnityEngine;

public class PortalManager : MonoBehaviour
{
    [Header("Prefabs (A/B 서로 다른 프리팹)")]
    public Portal portalPrefabA; // 파랑(A)
    public Portal portalPrefabB; // 빨강(B)

    [Header("Runtime (읽기용)")]
    public Portal portalA;
    public Portal portalB;

    void Awake()
    {
        if (!portalPrefabA || !portalPrefabB)
        {
            Debug.LogError("[PortalManager] portalPrefab 미할당!");
            enabled = false;
            return;
        }

        if (!portalA) portalA = Instantiate(portalPrefabA, Vector3.zero, Quaternion.identity, transform);
        if (!portalB) portalB = Instantiate(portalPrefabB, Vector3.right * 2f, Quaternion.identity, transform);

        // 서로 연결
        LinkBoth();
    }
    
    // === 외부에서 호출: PlayerController_Portal.cs가 사용 ===
    public void PlaceA(Vector3 pos, Quaternion rot)
    {
        // 기존 A 있으면 파괴 → 새로 생성
        if (portalA) Destroy(portalA.gameObject);
        portalA = Instantiate(portalPrefabA, pos, rot, transform);
        LinkBoth(); // B가 있다면 서로 target 재설정
    }

    public void PlaceB(Vector3 pos, Quaternion rot)
    {
        if (portalB) Destroy(portalB.gameObject);
        portalB = Instantiate(portalPrefabB, pos, rot, transform);
        LinkBoth();
    }

    // === 내부 유틸 ===
    void LinkBoth()
    {
        if (portalA) portalA.target = portalB;
        if (portalB) portalB.target = portalA;
    }
}
