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
	}
    ~InventorySystem() 
    {
        eventBrokerComponent.Unsubscribe<InventoryEvents.AddItem>(AddItemHandler);
		eventBrokerComponent.Unsubscribe<InventoryEvents.RemoveItem>(RemoveItemHandler);
	}

    private void AddItemHandler(BrokerEvent<InventoryEvents.AddItem> inEvent)
    {
		inventory.Add(inEvent.Payload.Item);
    }

	private void RemoveItemHandler(BrokerEvent<InventoryEvents.RemoveItem> inEvent)
	{
		
	}

}
