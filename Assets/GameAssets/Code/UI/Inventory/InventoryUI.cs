using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.UI;

public class InventoryUI : MonoBehaviour
{
    [SerializeField] private InventoryItemUI inventoryItemPrefab;
    [SerializeField] private Transform inventoryItemParent;
    [SerializeField] private RectTransform viewPort;
    [SerializeField] private Scrollbar scrollbar;
    [SerializeField] [Range(0f, 1f)] private float scrollDistance = 0.05f;

    [SerializeField] private Button collapseButton;
    [SerializeField] private Button expandButton;

    private List<InventoryItemUI> inventoryItemUIs = new List<InventoryItemUI>();

    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

    private void OnEnable()
    {
        eventBrokerComponent.Subscribe<InventoryEvents.AddItem>(AddItemHandler);
        eventBrokerComponent.Subscribe<InventoryEvents.RemoveItem>(RemoveItemHandler);
        eventBrokerComponent.Subscribe<InventoryEvents.ToggleInventoryVisibility>(ToggleInventoryVisibilityHandler);
        eventBrokerComponent.Subscribe<InventoryEvents.ScrollInventory>(ScrollInventoryHandler);
    }

    private void OnDisable()
    {
        eventBrokerComponent.Unsubscribe<InventoryEvents.AddItem>(AddItemHandler);
        eventBrokerComponent.Unsubscribe<InventoryEvents.RemoveItem>(RemoveItemHandler);
        eventBrokerComponent.Unsubscribe<InventoryEvents.ToggleInventoryVisibility>(ToggleInventoryVisibilityHandler);
        eventBrokerComponent.Unsubscribe<InventoryEvents.ScrollInventory>(ScrollInventoryHandler);
    }

    private void ScrollInventoryHandler(BrokerEvent<InventoryEvents.ScrollInventory> obj)
    {
        scrollbar.value = Mathf.Clamp01(scrollbar.value + (obj.Payload.Down ? -scrollDistance : scrollDistance));
    }

    private void AddItemHandler(BrokerEvent<InventoryEvents.AddItem> inEvent)
    {
        foreach (InventoryItem item in inEvent.Payload.Items)
        {
            if (inventoryItemUIs.Find(itemUI => itemUI.inventoryItem == item) != null) continue;
            InventoryItemUI itemUI = Instantiate(inventoryItemPrefab, inventoryItemParent);
            itemUI.Initialize(item, viewPort);
            inventoryItemUIs.Add(itemUI);
        }
        scrollbar.value = 0; // Scroll to most recently added item
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
