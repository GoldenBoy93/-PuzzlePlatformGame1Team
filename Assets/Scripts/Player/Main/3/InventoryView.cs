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

        nameText.text = string.IsNullOrEmpty(slot.ItemId.Value) ? "" : slot.ItemId.Value;
        descriptionText.text = $"Quantity: {slot.Quantity.Value}";

        useButton.SetActive(!string.IsNullOrEmpty(slot.ItemId.Value) && slot.ItemId.Value.Contains("Consumable"));
        equipButton.SetActive(!slot.IsEmpty.Value && !slot.Equipped.Value);
        unequipButton.SetActive(slot.Equipped.Value);
        dropButton.SetActive(!slot.IsEmpty.Value);
    }

    private void UpdateOutlineUI(int index)
    {
        for (int i = 0; i < slots.Length; i++)
        {
            var outline = slots[i].GetComponent<Outline>();
            if (outline == null)
            {
                // Outline이 없으면 자동으로 붙여줌
                outline = slots[i].gameObject.AddComponent<Outline>();
                outline.effectColor = Color.yellow;   // 원하는 색상
                outline.effectDistance = new Vector2(5, 5); // 두께
            }

            // 선택된 슬롯만 Outline 켜기
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