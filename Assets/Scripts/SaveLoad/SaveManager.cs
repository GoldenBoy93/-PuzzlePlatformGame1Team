using UnityEngine;
using System.IO;

public class SaveManager : MonoBehaviour
{
    // ���̺� ���� ��� ����
    private string saveFilePath;

    private void Awake()
    {
        // Application.persistentDataPath�� ��⺰�� �����Ͱ� �����ϰ� ����Ǵ� ��θ� ��ȯ
        saveFilePath = Path.Combine(Application.persistentDataPath, "player_data.json");
    }

    public void SaveGame()
    {
        PlayerData data = new PlayerData();

        // ���� ������ ä���
        //data.health = 100f;
        //data.playerName = "Hero";
        //data.playerPosition = GameObject.FindGameObjectWithTag("Player").transform.position;
        //data.inventoryItems.Add("Sword");
        //data.inventoryItems.Add("Shield");

        // �����͸� JSON ���ڿ��� ��ȯ
        string json = JsonUtility.ToJson(data, true); // true�� ������ ���� ������

        // ���Ͽ� JSON ���ڿ� ����
        File.WriteAllText(saveFilePath, json);
        Debug.Log("���� ���� �Ϸ�: " + saveFilePath);
    }
}