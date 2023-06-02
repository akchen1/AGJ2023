using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventorySystem 
{
	[SerializeField] private List<InventoryItem> inventory = new List<InventoryItem>();
	private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

	public InventorySystem() 
    {
        eventBrokerComponent.Subscribe<InventoryEvents.AddItem>(AddItemHandler);
		eventBrokerComponent.Subscribe<InventoryEvents.RemoveItem>(RemoveItemHandler);
		eventBrokerComponent.Subscribe<InventoryEvents.HasItem>(HasItemHandler);
	}


    ~InventorySystem() 
    {
        eventBrokerComponent.Unsubscribe<InventoryEvents.AddItem>(AddItemHandler);
		eventBrokerComponent.Unsubscribe<InventoryEvents.RemoveItem>(RemoveItemHandler);
		eventBrokerComponent.Unsubscribe<InventoryEvents.HasItem>(HasItemHandler);
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
        inEvent.Payload.Callback?.Invoke(inventory.Contains(inEvent.Payload.Item));
    }
}
