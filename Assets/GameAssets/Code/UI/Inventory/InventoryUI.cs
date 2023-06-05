using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private InventoryItemUI inventoryItemPrefab;
    [SerializeField] private Transform inventoryItemParent;

    private List<InventoryItemUI> inventoryItemUIs = new List<InventoryItemUI>();

    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

    private void OnEnable()
    {
        eventBrokerComponent.Subscribe<InventoryEvents.AddItem>(AddItemHandler);
        eventBrokerComponent.Subscribe<InventoryEvents.RemoveItem>(RemoveItemHandler);
    }

    private void OnDisable()
    {
        eventBrokerComponent.Unsubscribe<InventoryEvents.AddItem>(AddItemHandler);
        eventBrokerComponent.Unsubscribe<InventoryEvents.RemoveItem>(RemoveItemHandler);
    }

    private void AddItemHandler(BrokerEvent<InventoryEvents.AddItem> inEvent)
    {
        InventoryItemUI itemUI = Instantiate(inventoryItemPrefab, inventoryItemParent);
        itemUI.Initialize(inEvent.Payload.Item);
        inventoryItemUIs.Add(itemUI);
    }

    private void RemoveItemHandler(BrokerEvent<InventoryEvents.RemoveItem> inEvent)
    {
        foreach (InventoryItemUI item in inventoryItemUIs)
        {
            if (item.inventoryItem == inEvent.Payload.Item)
            {
                inventoryItemUIs.Remove(item);
                Destroy(item.gameObject);
                return;
            }
        }
    }
}
