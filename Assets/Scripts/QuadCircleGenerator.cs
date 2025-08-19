using UnityEngine;
#if UNITY_EDITOR
using UnityEditor;
#endif

public class QuadCircleGenerator : MonoBehaviour
{
    public GameObject quadPrefab;       
    public int count = 12;
    public float radius = 1.0f;
    public bool faceOutward = true;

    [Header("Safety")]
    public string containerName = "__Generated";         
    public bool clearOnlyGenerated = true;               

#if UNITY_EDITOR
    [ContextMenu("Generate Quad Circle (Safe)")]
    public void GenerateInEditor()
    {
        if (!quadPrefab) { Debug.LogWarning("quadPrefab이 비어있어요."); return; }

        
        Undo.RegisterFullObjectHierarchyUndo(gameObject, "Generate Quad Circle");

        
        Transform container = transform.Find(containerName);
        if (!container)
        {
            var go = new GameObject(containerName);
            Undo.RegisterCreatedObjectUndo(go, "Create Container");
            go.transform.SetParent(transform, false);
            container = go.transform;
        }

        
        if (clearOnlyGenerated)
        {
            for (int i = container.childCount - 1; i >= 0; i--)
            {
                var c = container.GetChild(i);
                if (c.GetComponent<GeneratedMarker>())
                    Undo.DestroyObjectImmediate(c.gameObject);
            }
        }

        
        for (int i = 0; i < Mathf.Max(1, count); i++)
        {
            float ang = i * Mathf.PI * 2f / Mathf.Max(1, count);
            Vector3 pos = new Vector3(Mathf.Cos(ang), Mathf.Sin(ang), 0) * radius;
            Quaternion rot = faceOutward
                ? Quaternion.LookRotation(Vector3.forward, pos.normalized)
                : Quaternion.identity;

            var go = (GameObject)PrefabUtility.InstantiatePrefab(quadPrefab);
            Undo.RegisterCreatedObjectUndo(go, "Instantiate Quad");
            go.transform.SetParent(container, false);
            go.transform.localPosition = pos;
            go.transform.localRotation = rot;
            go.name = $"Quad_{i}";

            
            if (!go.GetComponent<GeneratedMarker>())
                go.AddComponent<GeneratedMarker>();
        }
    }
#endif
}
