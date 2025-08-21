using System.Collections;
using System.Collections.Generic;
using TMPro;
using UniRx;
using UnityEngine;
using UnityEngine.UI;

public class InventoryView : MonoBehaviour
{
    public Transform slotPanel;
    public GameObject slotPrefab;
    public TextMeshProUGUI nameText;
    public TextMeshProUGUI descriptionText;
    public GameObject useButton;
    public GameObject equipButton;
    public GameObject unequipButton;
    public GameObject dropButton;

    private InventoryViewModel viewModel;
    private ItemSlot[] slots;
    private ReactiveProperty<int?> selectedIndex = new ReactiveProperty<int?>(null);
    private CompositeDisposable disposables = new CompositeDisposable();

    public void Init(InventoryViewModel vm)
    {
        viewModel = vm;

        // slotPanel 자식들을 슬롯으로 가져오기
        slots = slotPanel.GetComponentsInChildren<ItemSlot>();

        // ViewModel 바인딩
        for (int i = 0; i < slots.Length && i < viewModel.Slots.Count; i++)
        {
            slots[i].Init(viewModel.Slots[i], i, (index) => OnSelectItem(index));
        }

        // 첫 슬롯 선택
        if (slots.Length > 0)
            OnSelectItem(0);
    }

    private void OnSelectItem(int index)
    {
        selectedIndex.Value = index;
        UpdateSelectedUI(index);
        UpdateOutlineUI(index);
    }

    private void UpdateSelectedUI(int index)
    {
        var slot = viewModel.Slots[index].Slot;

        if (slot.IsEmpty.Value)
        {
            nameText.text = "";
            descriptionText.text = "";
            useButton.SetActive(false);
            equipButton.SetActive(false);
            unequipButton.SetActive(false);
            dropButton.SetActive(false);
            slots[index].SetImageActive(false); // 빈 슬롯 이미지 숨기기
            return;
        }

        nameText.text = slot.ItemId.Value;
        descriptionText.text = $"Quantity: {slot.Quantity.Value}";

        useButton.SetActive(slot.ItemId.Value.Contains("Consumable"));
        equipButton.SetActive(!slot.Equipped.Value);
        unequipButton.SetActive(slot.Equipped.Value);
        dropButton.SetActive(true);

        slots[index].SetImageActive(true);
    }

    private void UpdateOutlineUI(int index)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            var outline = slots[i].GetComponent<Outline>();
            if (outline == null)
            {
                outline = slots[i].gameObject.AddComponent<Outline>();
                outline.effectColor = Color.yellow;
                outline.effectDistance = new Vector2(5, 5);
            }

            outline.enabled = (i == index);
        }
    }

    public void OnEquipButton()
    {
        if (selectedIndex.Value == null) return;
        viewModel.Equip(selectedIndex.Value.Value);
    }

    public void OnUnEquipButton()
    {
        if (selectedIndex.Value == null) return;
        viewModel.UnEquip(selectedIndex.Value.Value);
    }

    public void OnDropButton()
    {
        if (selectedIndex.Value == null) return;
        viewModel.RemoveAt(selectedIndex.Value.Value);
    }

    private void OnDestroy() => disposables.Dispose();
}