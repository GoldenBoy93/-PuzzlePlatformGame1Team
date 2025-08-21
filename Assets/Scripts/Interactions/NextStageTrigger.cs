using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class NextStageTrigger : MonoBehaviour
{
    // ���� ���� ���� �� ���� ����
    private Scene currentScene;

    private void Awake()
    {
        // ���� ���� �����ͼ� ������ ����
        currentScene = SceneManager.GetActiveScene();
    }

    private void OnTriggerEnter(Collider other)
    {
        // �ݶ��̴��� ������ "�÷��̾�"���,
        if (other.CompareTag("Player"))
        {
            int scenebuildIndex = currentScene.buildIndex;

            // ���� ����ȣ�� ���� ���� �ҷ���
            SceneManager.LoadScene(scenebuildIndex + 1);

            if (scenebuildIndex == 2)
            { 
                //AudioManager.Instance.PlayBGM("Game1");
            }
        }
    }
}
