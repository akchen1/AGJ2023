using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private InventoryItemUI inventoryItemPrefab;
    [SerializeField] private Transform inventoryItemParent;

    [SerializeField] private Button collapseButton;
    [SerializeField] private Button expandButton;

    private List<InventoryItemUI> inventoryItemUIs = new List<InventoryItemUI>();

    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

    private void OnEnable()
    {
        eventBrokerComponent.Subscribe<InventoryEvents.AddItem>(AddItemHandler);
        eventBrokerComponent.Subscribe<InventoryEvents.RemoveItem>(RemoveItemHandler);
        eventBrokerComponent.Subscribe<InventoryEvents.ToggleInventoryVisibility>(ToggleInventoryVisibilityHandler);
    }

    private void OnDisable()
    {
        eventBrokerComponent.Unsubscribe<InventoryEvents.AddItem>(AddItemHandler);
        eventBrokerComponent.Unsubscribe<InventoryEvents.RemoveItem>(RemoveItemHandler);
        eventBrokerComponent.Unsubscribe<InventoryEvents.ToggleInventoryVisibility>(ToggleInventoryVisibilityHandler);
    }

    private void AddItemHandler(BrokerEvent<InventoryEvents.AddItem> inEvent)
    {
        foreach (InventoryItem item in inEvent.Payload.Items)
        {
            InventoryItemUI itemUI = Instantiate(inventoryItemPrefab, inventoryItemParent);
            itemUI.Initialize(item);
            inventoryItemUIs.Add(itemUI);
        }
    }

    private void RemoveItemHandler(BrokerEvent<InventoryEvents.RemoveItem> inEvent)
    {
        List<InventoryItemUI> uiToRemove = new List<InventoryItemUI>();
        foreach (InventoryItemUI item in inventoryItemUIs)
        {
            if (inEvent.Payload.Items.Contains(item.inventoryItem))
            {
                uiToRemove.Add(item);
            }
        }

        for (int i = uiToRemove.Count - 1; i >= 0; i--)
        {
            inventoryItemUIs.Remove(uiToRemove[i]);
            Destroy(uiToRemove[i].gameObject);
        }
    }

    private void ToggleInventoryVisibilityHandler(BrokerEvent<InventoryEvents.ToggleInventoryVisibility> inEvent)
    {
        if (inEvent.Payload.Visible)
        {
            expandButton.onClick.Invoke();
        } else
        {
            collapseButton.onClick.Invoke();
        }
    }
}
