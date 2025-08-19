using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interact : MonoBehaviour
{
    private ItemObject itemObject;
    public TextMeshProUGUI promptText;
    
    // ��ȣ�ۿ� Ʈ���� �ݶ��̴��� �θ𿡼� ItemObject ��� ��ũ��Ʈ�� ã�Ƽ� ������ ����
    void Start()
    {
        itemObject = GetComponentInParent<ItemObject>();
    }

    // �÷��̾ �ݶ��̴��� ������, Set������Ʈ �Լ� ȣ��
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("�ݶ��̴� Ȯ��");
            ActivePromptText();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("�ݶ��̴� ����2");
            NotActivePromptText();
        }
    }

    // �ش� �������� ������Ʈ ������ �����ͼ� ������ƮUI�� Ȱ��ȭ�ϸ鼭 ȭ�鿡 ���
    private void ActivePromptText()
    {
        promptText.gameObject.SetActive(true);
        promptText.text = itemObject.GetInteractPrompt();
    }

    // �ش� �������� ������Ʈ ������ �����ͼ� ������ƮUI�� Ȱ��ȭ�ϸ鼭 ȭ���� ��
    private void NotActivePromptText()
    {
        promptText.gameObject.SetActive(false);
        promptText.text = null;
    }
}