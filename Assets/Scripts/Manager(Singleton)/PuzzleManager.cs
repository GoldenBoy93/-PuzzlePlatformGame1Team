using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PuzzleManager : MonoBehaviour
{
    private static PuzzleManager _instance;

    public static PuzzleManager Instance
    {
        get
        {
            // �Ҵ���� �ʾ��� ��, �ܺο��� PuzzleManager.Instance �� �����ϴ� ���
            // ���� ������Ʈ�� ������ְ� PuzzleManager ��ũ��Ʈ�� AddComponent�� �ٿ��ش�.
            if (_instance == null)
            {
                // ���ӿ�����Ʈ�� ��� ���۽� ���°� Ȯ���� �Ŵ����� ���ӿ�����Ʈ�� ��������
                _instance = new GameObject("PuzzleManager").AddComponent<PuzzleManager>();
            }
            return _instance;
        }
    }

    private void Awake()
    {
        // Awake�� ȣ�� �� ����� �̹� �Ŵ��� ������Ʈ�� �����Ǿ� �ִ� ���̰�, '_instance'�� �ڽ��� �Ҵ�
        if (_instance == null)
        {
            _instance = this;
            DontDestroyOnLoad(gameObject);
        }
        else
        {
            // �̹� ������Ʈ�� �����ϴ� ��� '�ڽ�'�� �ı��ؼ� �ߺ�����
            if (_instance != this)
            {
                Destroy(gameObject);
            }
        }
    }

    // Key �־�� ������ ����
    public void KeyCheck(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            //Inventory playerInventory = other.GetComponent<Inventory>();

            ////�κ��丮�� ���谡 �ִ��� Ȯ��
            //if (playerInventory != null && playerInventory.HasItem(KeyName))
            //{
            //    Destroy(wallCollider);
            //    Debug.Log("���� �������ϴ�.");
            //    audioSource.PlayOneShot(soundEffect);
            //}
        }
    }
}
