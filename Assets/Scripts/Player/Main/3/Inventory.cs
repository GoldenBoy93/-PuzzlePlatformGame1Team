using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using Random = UnityEngine.Random;

public class Inventory : MonoBehaviour
{
    public List<ItemData> itemDatabase;

    public ItemSlot[] slots;
    public Transform slotPanel;

    [Header("Select Item")]
    public TextMeshProUGUI name;
    public TextMeshProUGUI description;
    public TextMeshProUGUI statName;
    public TextMeshProUGUI statValue;
    public GameObject useButton;
    public GameObject equipButton;
    public GameObject unequipButton;
    public GameObject dropButton;

    PlayerViewModel viewModel;
    public ItemData data;

    ItemData selectedItem;
    int selectedItemIndex = 0;
    int curEquipIndex;

    public PlayerModel _model;

    private void Start()
    {
        RefreshUI();

        slots = new ItemSlot[slotPanel.childCount];

        for (int i = 0; i < slots.Length; i++)
        {
            slots[i] = slotPanel.GetChild(i).GetComponent<ItemSlot>();
            slots[i].index = i;
            slots[i].inventory = this;
            slots[i].Clear();
        }
        Clear();
    }

    public void InitPanel()
    {
        _model = new PlayerModel();
        viewModel = UI_Manager.Instance._viewModel;
        viewModel.addItem += AddItem;
    }

    public void RefreshUI()
    {
        var items = _model.Inventory.Items;
        int i = 0;

        foreach (var kvp in items)
        {
            ItemData data = kvp.Key;
            int quantity = kvp.Value;

            slots[i].Set(data, quantity); // 아이템 전체랑 수량 넘김
            i++;
        }

        // 남은 슬롯 비우기
        for (; i < slots.Length; i++)
        {
            slots[i].Clear();
        }
    }
    void Clear()
    {
        selectedItem = null;
        name.text = string.Empty;
        description.text = string.Empty;
        statName.text = string.Empty;
        statValue.text = string.Empty;
        useButton.SetActive(false);
        equipButton.SetActive(false);
        unequipButton.SetActive(false);
        dropButton.SetActive(false);
    }
    public void AddItem()
    {
        if (data.stackable) //중복가능한지
        {
            ItemSlot slot = GetItemStack(data);
            if (slot != null)
            {
                slot.quantity++;
                UpdateUI();
                data = null;
                return;
            }
        }
        ItemSlot emptySlot = GetEmptySlot();
        if (emptySlot != null) //빈칸있는지
        {
            emptySlot.data = data;
            emptySlot.quantity = 1;
            UpdateUI();
            data = null;
            return;
        }
        data = null;
    }
    void UpdateUI()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].data != null) //슬롯에 데이터가 있다면 세팅해줘라
            {
                slots[i].Set(slots[i].data, slots[i].quantity, slots[i].equipped);
            }
            else
            {
                slots[i].Clear();
            }
        }
    }
    ItemSlot GetItemStack(ItemData data)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].data == data && slots[i].quantity < data.maxStack)
            {
                return slots[i];
            }
        }
        return null;
    }
    ItemSlot GetEmptySlot()
    {
        for (int i = 0; i < slots.Length; i++)
        {
            if (slots[i].data == null)
            {
                return slots[i];
            }
        }
        return null;
    }


    public void SelectItem(int index)
    {
        if (slots[index].data == null) return;
        
        selectedItem = slots[index].data;
        selectedItemIndex = index;
        
        name.text = selectedItem.displayName;
        description.text = selectedItem.description;


        statName.text = string.Empty;
        statValue.text = string.Empty;

        //for (int i = 0; i < selectedItem.consumables.Count; i++)
        //{
        //    statName.text += selectedItem.consumables[i].type.ToString() + "\n";
        //    statValue.text += selectedItem.consumables[i].value.ToString() + "\n";
        //}
        //
        //useButton.SetActive(selectedItem.type == ItemType.Consumable);
        //equipButton.SetActive(selectedItem.type == ItemType.Equipable && !slots[index].equipped);
        //unequipButton.SetActive(selectedItem.type == ItemType.Equipable && slots[index].equipped);
        //dropButton.SetActive(true);
    }
    //public void OnUseButton()
    //{
    //    if (selectedItem.type == ItemType.Consumable)
    //    {
    //        for (int i = 0; i < selectedItem.consumables.Count; i++)
    //        {
    //            switch (selectedItem.consumables[i].type)
    //            {
    //                case ConsumableType.Health:
    //                    condition.Heal(selectedItem.consumables[i].value);
    //                    break;
    //                case ConsumableType.Hunger:
    //                    condition.Eat(selectedItem.consumables[i].value);
    //                    break;
    //            }
    //        }
    //        RemoveSelectedItem();
    //    }
    //}


    void OnDropButton()
    {
        slots[selectedItemIndex].quantity--;
    
        if (slots[selectedItemIndex].quantity <= 0)
        {
            selectedItem = null;
            slots[selectedItemIndex].data = null;
            selectedItemIndex = -1;
            Clear();
        }
        UpdateUI();
    }
    public void OnEquipButton()
    {
        if (slots[curEquipIndex].equipped)
        {
            UnEquip(curEquipIndex);
        }
    
        slots[selectedItemIndex].equipped = true;
        curEquipIndex = selectedItemIndex;
        //GameManager.Instance.PlayerManager.equip.EquipNew(selectedItem);
        UpdateUI();
    
        SelectItem(selectedItemIndex);
    }
    void UnEquip(int index)
    {
        slots[index].equipped = false;
        //GameManager.Instance.PlayerManager.equip.UnEquip();
        UpdateUI();
    
        if (selectedItemIndex == index)
        {
            SelectItem(selectedItemIndex);
        }
    }
    public void OnUnEquipButton()
    {
        UnEquip(selectedItemIndex);
    }
}