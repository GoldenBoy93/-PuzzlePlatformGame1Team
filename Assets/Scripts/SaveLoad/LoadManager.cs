using UnityEngine;
using System.IO;

public class LoadManager : MonoBehaviour
{
    private string saveFilePath;

    private void Awake()
    {
        saveFilePath = Path.Combine(Application.persistentDataPath, "player_data.json");
    }

    public void LoadGame()
    {
        if (File.Exists(saveFilePath))
        {
            // 파일에서 JSON 문자열 읽기
            string json = File.ReadAllText(saveFilePath);

            // JSON 문자열을 PlayerData 객체로 변환
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);

            // 불러온 데이터 적용
            //Debug.Log("게임 불러오기 완료:");
            //Debug.Log("플레이어 체력: " + data.health);
            //Debug.Log("플레이어 이름: " + data.playerName);
            //Debug.Log("플레이어 위치: " + data.playerPosition);
            //foreach (string item in data.inventoryItems)
            //{
            //    Debug.Log("인벤토리 아이템: " + item);
            //}
        }
        else
        {
            Debug.LogWarning("세이브 파일이 존재하지 않습니다.");
        }
    }
}