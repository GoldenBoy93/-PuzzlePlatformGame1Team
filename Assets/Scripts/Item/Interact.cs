using TMPro;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interact : MonoBehaviour
{
    private ItemObject itemObject;
    public TextMeshProUGUI promptText;
    
    // 상호작용 트리거 콜라이더의 부모에서 ItemObject 라는 스크립트를 찾아서 변수에 저장
    void Start()
    {
        itemObject = GetComponentInParent<ItemObject>();
    }

    // 플레이어가 콜라이더에 닿으면, Set프롬프트 함수 호출
    private void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("콜라이더 확인");
            ActivePromptText();
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            Debug.Log("콜라이더 나감2");
            NotActivePromptText();
        }
    }

    // 해당 아이템의 프롬프트 정보를 가져와서 프롬프트UI를 활성화하면서 화면에 띄움
    private void ActivePromptText()
    {
        promptText.gameObject.SetActive(true);
        promptText.text = itemObject.GetInteractPrompt();
    }

    // 해당 아이템의 프롬프트 정보를 가져와서 프롬프트UI를 활성화하면서 화면을 끔
    private void NotActivePromptText()
    {
        promptText.gameObject.SetActive(false);
        promptText.text = null;
    }
}