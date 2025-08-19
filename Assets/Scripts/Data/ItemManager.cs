using System.Collections.Generic;
using UnityEngine;

public class ItemManager : MonoBehaviour
{
    public static ItemManager Instance { get; private set; }

    [Tooltip("���ӿ��� ���� ��� ItemData�� ���⿡ ��� (�Ǵ� Resources/Addressables �ε�)")]
    public List<ItemData> items = new();

    Dictionary<string, ItemData> map;

    void Awake()
    {
        if (Instance && Instance != this)
        {
            Destroy(gameObject); return; 
        }
        
        Instance = this;
        
        DontDestroyOnLoad(gameObject);

        map = new Dictionary<string, ItemData>(items.Count);
        foreach (var it in items)
            if (!string.IsNullOrEmpty(it.id)) map[it.id] = it;
    }

    public ItemData GetById(string id) 
    {
        if (id != null && map.TryGetValue(id, out var d))
            return d;
        else
            return null; 
    }
}
