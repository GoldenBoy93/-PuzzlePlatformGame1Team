using UnityEngine;
using System.IO;

public class SaveManager : MonoBehaviour
{
    // 세이브 파일 경로 설정
    private string saveFilePath;

    private void Awake()
    {
        // Application.persistentDataPath는 기기별로 데이터가 안전하게 저장되는 경로를 반환
        saveFilePath = Path.Combine(Application.persistentDataPath, "player_data.json");
    }

    public void SaveGame()
    {
        PlayerData data = new PlayerData();

        // 예시 데이터 채우기
        //data.health = 100f;
        //data.playerName = "Hero";
        //data.playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
        //data.inventoryItems.Add("Sword");
        //data.inventoryItems.Add("Shield");

        // 데이터를 JSON 문자열로 변환
        string json = JsonUtility.ToJson(data, true); // true는 가독성 좋게 포맷팅

        // 파일에 JSON 문자열 쓰기
        File.WriteAllText(saveFilePath, json);
        Debug.Log("게임 저장 완료: " + saveFilePath);
    }
}