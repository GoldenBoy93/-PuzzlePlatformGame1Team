using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public ItemData data;
    public Inventory inventory;
    public Button button;
    public Image icon;
    public TextMeshProUGUI quantityText;
    private Outline outline;

    [HideInInspector] public int index;
    //[HideInInspector] public InventoryView inventoryView;

    public string itemName;
    public int quantity;
    public bool equipped;



    private void Awake() => outline = GetComponent<Outline>();

    private void OnEnable() => outline.enabled = equipped;


    public void Set(string itemName, int quantity, bool equipped = false)
    {
        this.itemName = itemName;
        this.quantity = quantity;
        this.equipped = equipped;

        //var data = GetItemData(itemName);
        if (data == null) return;
        
        icon.gameObject.SetActive(true);
        icon.sprite = data.icon;
        quantityText.text = quantity > 1 ? quantity.ToString() : "";
        if (outline != null) outline.enabled = equipped;
    }

    public void Clear()
    {
        itemName = null;
        quantity = 0;
        equipped = false;
        icon.gameObject.SetActive(false);
        quantityText.text = "";
        if (outline != null) outline.enabled = false;
    }

    public void OnClickButton() => inventory.SelectItem(index);


    //public ItemData GetItemData(string name)
    //{
    //    if (string.IsNullOrEmpty(name)) return null;
    //    return data.displayName(name);
    //}
}
