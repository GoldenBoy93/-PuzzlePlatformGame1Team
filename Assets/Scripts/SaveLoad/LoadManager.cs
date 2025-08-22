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
            // ���Ͽ��� JSON ���ڿ� �б�
            string json = File.ReadAllText(saveFilePath);

            // JSON ���ڿ��� PlayerData ��ü�� ��ȯ
            PlayerData data = JsonUtility.FromJson<PlayerData>(json);

            // �ҷ��� ������ ����
            //Debug.Log("���� �ҷ����� �Ϸ�:");
            //Debug.Log("�÷��̾� ü��: " + data.health);
            //Debug.Log("�÷��̾� �̸�: " + data.playerName);
            //Debug.Log("�÷��̾� ��ġ: " + data.playerPosition);
            //foreach (string item in data.inventoryItems)
            //{
            //    Debug.Log("�κ��丮 ������: " + item);
            //}
        }
        else
        {
            Debug.LogWarning("���̺� ������ �������� �ʽ��ϴ�.");
        }
    }
}