using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class PlayerView : MonoBehaviour
{
    public Image healthImage;
    public Image staminaImage;
    public Transform inventoryContent; // 아이템 UI를 자식으로 배치할 컨테이너
    public GameObject inventoryItemPrefab; // 아이템 UI 프리팹

    private PlayerViewModel viewModel;



    void Start()
    {
        viewModel = UI_Manager.Instance._viewModel;

        // 체력 / 스태미나 구독 (FillAmount 업데이트)
        viewModel.Health.Subscribe(value =>
        {
            healthImage.fillAmount = (float)value / viewModel.MaxHealth;
        });

        viewModel.Stamina.Subscribe(value =>
        {
            staminaImage.fillAmount = (float)value / viewModel.MaxStamina;
        });

        // 인벤토리 UI 구독
        viewModel.Inventory.ObserveAdd().Subscribe(e => OnItemAdded(e.Key, e.Value));
        viewModel.Inventory.ObserveRemove().Subscribe(e => OnItemRemoved(e.Key));
        viewModel.Inventory.ObserveReplace().Subscribe(e => OnItemUpdated(e.Key, e.NewValue));
    }



    void Update()
    {
        // 데미지 / 회복 테스트
        if (Input.GetKeyDown(KeyCode.H)) viewModel.TakeDamage(1);
        if (Input.GetKeyDown(KeyCode.J)) viewModel.Heal(2);

        // 달리기 스태미나 소모
        if (Input.GetKey(KeyCode.LeftShift)) viewModel.ConsumeStamina(1);

    }




    private void OnItemAdded(ItemData data, int amount)
    {
        var go = Instantiate(inventoryItemPrefab, inventoryContent);
        go.name = data.id; // 고유 ID 사용

        // 슬롯 UI 세팅
        var slot = go.GetComponent<ItemSlot>();
        if (slot != null)
        {
            slot.Set(data, amount);
        }
        else
        {
            // 예: 단순 텍스트 UI일 경우
            go.GetComponentInChildren<Text>().text = $"{data.displayName} x{amount}";
        }
    }

    private void OnItemRemoved(ItemData data)
    {
        var go = inventoryContent.Find(data.id);
        if (go != null) Destroy(go.gameObject);
    }

    private void OnItemUpdated(ItemData data, int amount)
    {
        var go = inventoryContent.Find(data.id);
        if (go != null)
        {
            var slot = go.GetComponent<ItemSlot>();
            if (slot != null)
                slot.Set(data, amount);
            else
                go.GetComponentInChildren<Text>().text = $"{data.displayName} x{amount}";
        }
    }




    //viewModel.CurrentEquipChanged.Subscribe(equip =>
    //{
    //if (equip != null)
    //    animator.SetTrigger("Equip");
    //else
    //    animator.SetTrigger("Unequip");
    //});






}