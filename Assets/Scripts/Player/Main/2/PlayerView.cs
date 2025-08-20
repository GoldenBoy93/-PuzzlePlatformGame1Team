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

        // 인벤토리 테스트
        if (Input.GetKeyDown(KeyCode.Alpha1)) viewModel.AddItem("Potion", 1);
        if (Input.GetKeyDown(KeyCode.Alpha2)) viewModel.RemoveItem("Potion", 1);
    }




    private void OnItemAdded(string itemName, int amount)
    {
        var go = Instantiate(inventoryItemPrefab, inventoryContent);
        go.GetComponentInChildren<Text>().text = $"{itemName} x{amount}";
        go.name = itemName;
    }

    private void OnItemRemoved(string itemName)
    {
        var go = inventoryContent.Find(itemName);
        if (go != null) Destroy(go.gameObject);
    }

    private void OnItemUpdated(string itemName, int amount)
    {
        var go = inventoryContent.Find(itemName);
        if (go != null) go.GetComponentInChildren<Text>().text = $"{itemName} x{amount}";
    }





   //viewModel.CurrentEquipChanged.Subscribe(equip =>
   //{
   //if (equip != null)
   //    animator.SetTrigger("Equip");
   //else
   //    animator.SetTrigger("Unequip");
   //});






}