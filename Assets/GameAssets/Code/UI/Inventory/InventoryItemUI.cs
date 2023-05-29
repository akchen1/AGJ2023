using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class InventoryItemUI : MonoBehaviour
{
    private Image itemIcon;
    private InventoryItem inventoryItem;

    private void Awake()
    {
        itemIcon = GetComponent<Image>();
    }

    public void Initialize(InventoryItem inventoryItem)
    {
        this.inventoryItem = inventoryItem;
        itemIcon.sprite = inventoryItem.ItemIcon;
    }
}
