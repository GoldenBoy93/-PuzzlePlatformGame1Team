using UnityEngine;

public class PortalManager : MonoBehaviour
{
    [Header("Prefabs (A/B ���� �ٸ� ������)")]
    public Portal portalPrefabA; // �Ķ�(A)
    public Portal portalPrefabB; // ����(B)

    [Header("Runtime (�б��)")]
    public Portal portalA;
    public Portal portalB;

    void Awake()
    {
        if (!portalPrefabA || !portalPrefabB)
        {
            Debug.LogError("[PortalManager] portalPrefab ���Ҵ�!");
            enabled = false;
            return;
        }

        if (!portalA) portalA = Instantiate(portalPrefabA, Vector3.zero, Quaternion.identity, transform);
        if (!portalB) portalB = Instantiate(portalPrefabB, Vector3.right * 2f, Quaternion.identity, transform);

        // ���� ����
        LinkBoth();
    }
    
    // === �ܺο��� ȣ��: PlayerController_Portal.cs�� ��� ===
    public void PlaceA(Vector3 pos, Quaternion rot)
    {
        // ���� A ������ �ı� �� ���� ����
        if (portalA) Destroy(portalA.gameObject);
        portalA = Instantiate(portalPrefabA, pos, rot, transform);
        LinkBoth(); // B�� �ִٸ� ���� target �缳��
    }

    public void PlaceB(Vector3 pos, Quaternion rot)
    {
        if (portalB) Destroy(portalB.gameObject);
        portalB = Instantiate(portalPrefabB, pos, rot, transform);
        LinkBoth();
    }

    // === ���� ��ƿ ===
    void LinkBoth()
    {
        if (portalA) portalA.target = portalB;
        if (portalB) portalB.target = portalA;
    }
}
