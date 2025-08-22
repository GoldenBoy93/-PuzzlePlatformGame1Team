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
    public float interactionRadius = 2f; // 상호작용 범위
    private List<ItemObject> itemsInRange = new List<ItemObject>();

    private void Start()
    {
        // UI_Manager에서 프롬프트 가져오기
        promptText = UI_Manager.Instance._promptText;
        text = promptText.GetComponentInChildren<TextMeshProUGUI>();
        promptText.SetActive(false);
    }

    private void Update()
    {
        // 매 프레임 플레이어 주변 범위 체크
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

        // 범위 내 아이템이 있으면 프롬프트 표시
        if (itemsInRange.Count > 0)
        {
            promptText.SetActive(true);
            text.text = itemsInRange[itemsInRange.Count - 1].GetInteractPrompt(); // 가장 가까운 아이템
        }
        else
        {
            promptText.SetActive(false);
        }

        // E 키 입력 시 가장 가까운 아이템 상호작용
        if (Input.GetKeyDown(KeyCode.E) && itemsInRange.Count > 0)
        {
            // 주울 수 있는 아이템인지 확인
            bool getable = itemsInRange[itemsInRange.Count - 1].CheckGetable();

            if (getable)
            {
                var item = itemsInRange[itemsInRange.Count - 1];
                item.OnInteract(gameObject); // Player 전달
            }
            return;
        }
    }

    // 선택 사항: 시각화용
    private void OnDrawGizmosSelected()
    {
        Gizmos.color = Color.green;
        Gizmos.DrawWireSphere(transform.position, interactionRadius);
    }
}