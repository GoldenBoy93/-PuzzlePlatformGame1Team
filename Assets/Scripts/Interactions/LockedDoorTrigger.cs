using System.Linq;
using TMPro;
using UnityEngine;

public class LockedDoorTrigger : MonoBehaviour
{
    public InventoryViewModel inventoryVM; // 플레이어 인벤토리
    public ItemType requiredKey;           // 이 문을 여는 키
    private bool isOpen = false;
    public TextMeshProUGUI text;

    private void OnTriggerEnter(Collider other)
    {
        if (isOpen) return;
        if (!other.CompareTag("Player")) return;

        // 플레이어 인벤토리에 requiredKey 있는지 체크
        bool hasKey = inventoryVM.Slots
            .Any(s => s.Slot.Item.Value != null && s.Slot.Item.Value.type == requiredKey);

        if (hasKey)
        {
            Open();
        }
        else
        {
            text.text = ($"문을 열기 위해 {requiredKey} 키가 필요합니다.");
        }
    }

    private void Open()
    {
        isOpen = true;
        text.text = ($"{requiredKey} 키로 문 열림!");
        // 필요 시 애니메이션, 사운드, 충돌체 비활성화 등 처리
        Destroy(gameObject); // 문 오브젝트 제거
    }
}