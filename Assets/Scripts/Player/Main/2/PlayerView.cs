using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UniRx;

public class PlayerView : MonoBehaviour
{
    public Image healthImage;
    public Image staminaImage;
    public Transform inventoryContent; // ������ UI�� �ڽ����� ��ġ�� �����̳�
    public GameObject inventoryItemPrefab; // ������ UI ������

    private PlayerViewModel viewModel;



    void Start()
    {
        viewModel = UI_Manager.Instance._viewModel;

        // ü�� / ���¹̳� ���� (FillAmount ������Ʈ)
        viewModel.Health.Subscribe(value =>
        {
            healthImage.fillAmount = (float)value / viewModel.MaxHealth;
        });

        viewModel.Stamina.Subscribe(value =>
        {
            staminaImage.fillAmount = (float)value / viewModel.MaxStamina;
        });

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

    }




    private void OnItemAdded(ItemData data, int amount)
    {
        var go = Instantiate(inventoryItemPrefab, inventoryContent);
        go.name = data.id; // ���� ID ���

        // ���� UI ����
        var slot = go.GetComponent<ItemSlot>();
        if (slot != null)
        {
            slot.Set(data, amount);
        }
        else
        {
            // ��: �ܼ� �ؽ�Ʈ UI�� ���
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