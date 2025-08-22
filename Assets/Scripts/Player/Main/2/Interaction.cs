using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.InputSystem;
using UnityEngine.UI;


public interface IInteractable
{
    public string GetInteractPrompt();
    public void Oninteract();
}

public class Interaction : MonoBehaviour //,IInteractable
{
    [Header("UI")]
    private GameObject promptText;
    private TextMeshProUGUI text;

    [Header("Interaction Settings")]
    public float interactionRadius = 2f; // ��ȣ�ۿ� ����
    private List<ItemObject> itemsInRange = new List<ItemObject>();

    private void Start()
    {
        // UI_Manager���� ������Ʈ ��������
        promptText = UI_Manager.Instance._promptText;
        text = promptText.GetComponentInChildren<TextMeshProUGUI>();
        promptText.SetActive(false);
    }

    private void Update()
    {
        // �� ������ �÷��̾� �ֺ� ���� üũ
        itemsInRange.Clear();
        Collider[] hits = Physics.OverlapSphere(transform.position, interactionRadius);
        foreach (var hit in hits)
        {
            var item = hit.GetComponent<ItemObject>();
            if (item != null)
            {
                itemsInRange.Add(item);
            }
        }

        // ���� �� �������� ������ ������Ʈ ǥ��
        if (itemsInRange.Count > 0)
        {
            promptText.SetActive(true);
            text.text = itemsInRange[itemsInRange.Count - 1].GetInteractPrompt(); // ���� ����� ������
        }
        else
        {
            promptText.SetActive(false);
        }

        // E Ű �Է� �� ���� ����� ������ ��ȣ�ۿ�
        if (Input.GetKeyDown(KeyCode.E) && itemsInRange.Count > 0)
        {
            // �ֿ� �� �ִ� ���������� Ȯ��
            bool getable = itemsInRange[itemsInRange.Count - 1].CheckGetable();

            if (getable)
            {
                var item = itemsInRange[itemsInRange.Count - 1];
                item.OnInteract(gameObject); // Player ����
            }
            return;
        }
    }

    // ���� ����: �ð�ȭ��
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}