using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class PlayerView : MonoBehaviour
{
    public Slider healthSlider;
    public Slider staminaSlider;
    public Transform inventoryContent; // ������ UI�� �ڽ����� ��ġ�� �����̳�
    public GameObject inventoryItemPrefab; // ������ UI ������

    private PlayerViewModel viewModel;

    void Start()
    {
        var model = new PlayerModel();
        viewModel = new PlayerViewModel(model);

        // Slider �ִ밪 ����
        healthSlider.maxValue = model.MaxHealth;
        staminaSlider.maxValue = model.MaxStamina;

        // ü�� / ���¹̳� ����
        viewModel.Health.Subscribe(value => healthSlider.value = value);
        viewModel.Stamina.Subscribe(value => staminaSlider.value = value);

        // �κ��丮 UI ����
        viewModel.Inventory.ObserveAdd().Subscribe(e => OnItemAdded(e.Key, e.Value));
        viewModel.Inventory.ObserveRemove().Subscribe(e => OnItemRemoved(e.Key));
        viewModel.Inventory.ObserveReplace().Subscribe(e => OnItemUpdated(e.Key, e.NewValue));
    }



    void Update()
    {
        // ������ / ȸ�� �׽�Ʈ
        if (Input.GetKeyDown(KeyCode.H)) viewModel.TakeDamage(1);
        if (Input.GetKeyDown(KeyCode.J)) viewModel.Heal(2);

        // �޸��� ���¹̳� �Ҹ�
        if (Input.GetKey(KeyCode.LeftShift)) viewModel.ConsumeStamina(1);

        // �κ��丮 �׽�Ʈ
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
}