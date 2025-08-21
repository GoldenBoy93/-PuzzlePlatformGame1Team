using System.Collections;
using System.Collections.Generic;
using System.Linq;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class ItemSlot : MonoBehaviour
{
    public Inventory inventory;
    public Button button;
    public Image icon;
    public TextMeshProUGUI quantityText;
    private Outline outline;

    [HideInInspector] public int index;
    //[HideInInspector] public InventoryView inventoryView;

    public ItemData data;
    public string itemName;
    public int quantity;
    public bool equipped;



    private void Awake() => outline = GetComponent<Outline>();

    private void OnEnable()
    {
        if (data == null) outline.enabled = false;
        else outline.enabled = equipped;
    }

    public void Set(ItemData data, int quantity, bool choice = false)
    {
        this.data = data;
        this.itemName = data.displayName;
        this.quantity = quantity;
        this.equipped = choice;

        icon.gameObject.SetActive(true);
        icon.sprite = data.icon;
        quantityText.text = quantity > 1 ? quantity.ToString() : "";
        if (outline != null) outline.enabled = choice;
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


}
