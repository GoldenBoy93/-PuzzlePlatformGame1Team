using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TempGameManager : MonoBehaviour
{
    public ItemSlot itemSlot;
    public PortalGun portalGun;

    // Start is called before the first frame update
    void Awake()
    {
        itemSlot = GetComponent<ItemSlot>();
    }

    private void Start()
    {
        itemSlot.EquipByItemId(portalGunData.id);
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
