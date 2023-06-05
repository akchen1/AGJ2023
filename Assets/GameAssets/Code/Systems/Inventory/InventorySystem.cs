using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEditor.UIElements;
using UnityEngine;

[System.Serializable]
public class InventorySystem 
{
	[SerializeField] private List<InventoryItem> inventory = new List<InventoryItem>();
	private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

	private InventoryItem selectedItem;

	public InventorySystem() 
    {
        eventBrokerComponent.Subscribe<InventoryEvents.AddItem>(AddItemHandler);
		eventBrokerComponent.Subscribe<InventoryEvents.RemoveItem>(RemoveItemHandler);
		eventBrokerComponent.Subscribe<InventoryEvents.HasItem>(HasItemHandler);
		eventBrokerComponent.Subscribe<InventoryEvents.SelectItem>(SelectItemHandler);
		eventBrokerComponent.Subscribe<InventoryEvents.DragCombineItem>(DragCombineItemHandler);
	}

    

    ~InventorySystem() 
    {
        eventBrokerComponent.Unsubscribe<InventoryEvents.AddItem>(AddItemHandler);
		eventBrokerComponent.Unsubscribe<InventoryEvents.RemoveItem>(RemoveItemHandler);
		eventBrokerComponent.Unsubscribe<InventoryEvents.HasItem>(HasItemHandler);
		eventBrokerComponent.Unsubscribe<InventoryEvents.SelectItem>(SelectItemHandler);
		eventBrokerComponent.Unsubscribe<InventoryEvents.DragCombineItem>(DragCombineItemHandler);
	}

    private void AddItemHandler(BrokerEvent<InventoryEvents.AddItem> inEvent)
    {
		if (inventory.Contains(inEvent.Payload.Item)) return;
		inventory.Add(inEvent.Payload.Item);
    }

	private void RemoveItemHandler(BrokerEvent<InventoryEvents.RemoveItem> inEvent)
	{
		if (inventory.Contains(inEvent.Payload.Item))
		{
			inventory.Remove(inEvent.Payload.Item);
		}
	}

    private void HasItemHandler(BrokerEvent<InventoryEvents.HasItem> inEvent)
    {
		foreach (InventoryItem item in inEvent.Payload.Item)
		{
			if (!inventory.Contains(item))
			{
				inEvent.Payload.Callback?.Invoke(false);
				return;
			}
		}
        inEvent.Payload.Callback?.Invoke(true);
    }

    private void SelectItemHandler(BrokerEvent<InventoryEvents.SelectItem> inEvent)
    {
		InventoryItem item = inEvent.Payload.Item;
		selectedItem = item;
		selectedItem.OnSelectInteraction();
    }

    private void DragCombineItemHandler(BrokerEvent<InventoryEvents.DragCombineItem> inEvent)
    {
		Debug.Log("drag combine called");
		InventoryItem item1 = inEvent.Payload.Item1;
		InventoryItem item2 = inEvent.Payload.Item2;

		if (!item1.InventoryInteraction.RequiredItems.Contains(item2)) return;
		if (!item2.InventoryInteraction.RequiredItems.Contains(item1)) return;
        item1.InventoryInteraction.OnDragInteraction();

    }

    private void CheckItemInteraction(InventoryItem item)
    {
		if (item == null) return;
        if (item.InventoryInteraction == null) return;
		//if (item.InventoryInteraction.RequiredItemInteraction && !ContainsRequiredItems(item)) return;
		item.InventoryInteraction.Interact();
    }

  //  private bool ContainsRequiredItems(InventoryItem item)
  //  {
		//return inventory.Contains(item.InventoryInteraction.RequiredItem);
  //  }
}
