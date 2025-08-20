using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SafeFetchHelper
{

    //this�ϸ� ��ü�����ؿ�, early return���ϸ� �̻��Ѱ����� ����
    //���⼱ go = this


    /// <summary>
    /// 1. ���� GameObject���� ������Ʈ �������� + ���� �α�
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="go"></param>
    /// <returns></returns>
    public static T GetOrError<T>(GameObject go) where T : Component
    {
        if (go.TryGetComponent<T>(out T comp))
            return comp;

        Debug.LogError($"[{go.name}]���� {typeof(T).Name} UI ������Ʈ�� ã�� �� �����ϴ�!", go);
        return null;
    }

    public static T GetChildOrError<T>(GameObject go) where T : Component
    {
        T comp = go.GetComponentInChildren<T>(true);
        if (comp != null)
            return comp;

        Debug.LogError($"[{go.name}] (�ڽ� ����)���� {typeof(T).Name} UI ������Ʈ�� ã�� �� �����ϴ�!", go);
        return null;
    }


    /// <summary>
    /// ������ T Ÿ�� �̱��� ��ü ��������, ������ ���� ����
    /// </summary>
    public static T GetOrCreate<T>(string objectName = null) where T : Component
    {
        T instance = Object.FindObjectOfType<T>();

        if (instance != null) return instance;

        // ������ ���� ����
        string goName = objectName ?? typeof(T).Name;
        GameObject go = new GameObject(goName);
        instance = go.AddComponent<T>();

        // �� ��ȯ���� �ı����� �ʵ���
        Object.DontDestroyOnLoad(go);

        Debug.LogWarning($"���� {typeof(T).Name}��(��) ��� ���� �����߽��ϴ�: {goName}");
        return instance;
    }

    /// <summary>
    /// 2. ������ ��ü ã�Ƽ� ������Ʈ ��������, ������ ����
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="objectName"></param>
    /// <returns></returns>
    public static T GetOrCreateByName<T>(string objectName) where T : Component
    {
        GameObject go = GameObject.Find(objectName);
        if (go == null)
        {
            Debug.LogWarning($"'{objectName}' GameObject�� ��� ���� �����մϴ�.");
            go = new GameObject(objectName);
        }

        T comp = go.GetComponent<T>();
        if (comp == null)
        {
            Debug.LogWarning($"'{objectName}'�� {typeof(T).Name} ������Ʈ�� ��� ���� �߰��մϴ�.");
            comp = go.AddComponent<T>();
        }

        return comp;
    }

    /// <summary>
    /// ������ MainCamera �����ϰ� ��������
    /// </summary>
    public static Camera GetMainCamera()
    {
        Camera cam = Camera.main;
        if (cam == null)
            Debug.LogError("���� MainCamera�� �����ϴ�!");
        return cam;
    }

    /// <summary>
    /// 3. ������ �ܼ� ������Ʈ �˻�, ������ ���
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T FindOrError<T>() where T : Component
    {
        T comp = Object.FindObjectOfType<T>();
        if (comp == null)
            Debug.LogError($"������ {typeof(T).Name} ������Ʈ�� ã�� �� �����ϴ�!");
        return comp;
    }

}
