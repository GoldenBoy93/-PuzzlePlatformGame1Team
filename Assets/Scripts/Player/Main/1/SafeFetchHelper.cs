using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class SafeFetchHelper
{

    //this하면 물체참조해옴, early return안하면 이상한곳에서 터짐
    //여기선 go = this


    /// <summary>
    /// 1. 같은 GameObject에서 컴포넌트 가져오기 + 에러 로그
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="go"></param>
    /// <returns></returns>
    public static T GetOrError<T>(GameObject go) where T : Component
    {
        if (go.TryGetComponent<T>(out T comp))
            return comp;

        Debug.LogError($"[{go.name}]에서 {typeof(T).Name} UI 컴포넌트를 찾을 수 없습니다!", go);
        return null;
    }

    public static T GetChildOrError<T>(GameObject go) where T : Component
    {
        T comp = go.GetComponentInChildren<T>(true);
        if (comp != null)
            return comp;

        Debug.LogError($"[{go.name}] (자식 포함)에서 {typeof(T).Name} UI 컴포넌트를 찾을 수 없습니다!", go);
        return null;
    }


    /// <summary>
    /// 씬에서 T 타입 싱글톤 객체 가져오기, 없으면 새로 생성
    /// </summary>
    public static T GetOrCreate<T>(string objectName = null) where T : Component
    {
        T instance = Object.FindObjectOfType<T>();

        if (instance != null) return instance;

        // 없으면 새로 생성
        string goName = objectName ?? typeof(T).Name;
        GameObject go = new GameObject(goName);
        instance = go.AddComponent<T>();

        // 씬 전환에도 파괴되지 않도록
        Object.DontDestroyOnLoad(go);

        Debug.LogWarning($"씬에 {typeof(T).Name}이(가) 없어서 새로 생성했습니다: {goName}");
        return instance;
    }

    /// <summary>
    /// 2. 씬에서 객체 찾아서 컴포넌트 가져오기, 없으면 생성
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <param name="objectName"></param>
    /// <returns></returns>
    public static T GetOrCreateByName<T>(string objectName) where T : Component
    {
        GameObject go = GameObject.Find(objectName);
        if (go == null)
        {
            Debug.LogWarning($"'{objectName}' GameObject가 없어서 새로 생성합니다.");
            go = new GameObject(objectName);
        }

        T comp = go.GetComponent<T>();
        if (comp == null)
        {
            Debug.LogWarning($"'{objectName}'에 {typeof(T).Name} 컴포넌트가 없어서 새로 추가합니다.");
            comp = go.AddComponent<T>();
        }

        return comp;
    }

    /// <summary>
    /// 씬에서 MainCamera 안전하게 가져오기
    /// </summary>
    public static Camera GetMainCamera()
    {
        Camera cam = Camera.main;
        if (cam == null)
            Debug.LogError("씬에 MainCamera가 없습니다!");
        return cam;
    }

    /// <summary>
    /// 3. 씬에서 단순 컴포넌트 검색, 없으면 경고
    /// </summary>
    /// <typeparam name="T"></typeparam>
    /// <returns></returns>
    public static T FindOrError<T>() where T : Component
    {
        T comp = Object.FindObjectOfType<T>();
        if (comp == null)
            Debug.LogError($"씬에서 {typeof(T).Name} 컴포넌트를 찾을 수 없습니다!");
        return comp;
    }

}
