using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class UI_Manager : MonoBehaviour
{
    public List<GameObject> objectPrefabs;  // 프리팹 5개 넣기
    public float spawnRangeX = 5f;
    public float spawnHeight = 6f; // 화면 위에서 생성
    public int spawnCountPerPrefab = 5; // 프리팹당 생성 개수
    public float spawnInterval = 0.3f; // 오브젝트 하나씩 떨어지는 간격

    void Start()
    {
        StartCoroutine(SpawnObjectsGradually());
    }
    void Update()
    {
    }
    IEnumerator SpawnObjectsGradually()
    {
        while (true)
        {
            // 랜덤한 프리팹 하나 선택
            GameObject prefab = objectPrefabs[Random.Range(0, objectPrefabs.Count)];

            float spawnX = Random.Range(-spawnRangeX, spawnRangeX);
            Vector3 spawnPos = new Vector3(spawnX, spawnHeight, 0);

            GameObject obj = Instantiate(prefab, spawnPos, Quaternion.identity);
            obj.AddComponent<FallingObjectCleaner>(); // 여기에서 추가!

            yield return new WaitForSeconds(spawnInterval);
        }
    }

    public void OnClickStart()
    {

    }

    public void OnClickQuit()
    {
        Application.Quit();
        Debug.Log("게임 종료"); // 에디터에서는 작동 안함, 빌드해야 확인 가능
    }
}
public class FallingObjectCleaner : MonoBehaviour
{
    public float destroyY = -6f;

    void Update()
    {
        if (transform.position.y < destroyY)
        {
            Destroy(gameObject);
        }
    }
}
