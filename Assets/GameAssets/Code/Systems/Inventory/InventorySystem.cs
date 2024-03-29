using DS.ScriptableObjects;
using System;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventorySystem 
{
	[SerializeField] private HashSet<InventoryItem> inventory = new HashSet<InventoryItem>();
	[SerializeField] private DSDialogueSO invalidCombinationDialogue;

	private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

	private InventoryItem selectedItem;

	public InventorySystem() 
    {
        eventBrokerComponent.Subscribe<InventoryEvents.AddItem>(AddItemHandler);
		eventBrokerComponent.Subscribe<InventoryEvents.RemoveItem>(RemoveItemHandler);
		eventBrokerComponent.Subscribe<InventoryEvents.HasItem>(HasItemHandler);
		eventBrokerComponent.Subscribe<InventoryEvents.SelectItem>(SelectItemHandler);
		eventBrokerComponent.Subscribe<InventoryEvents.DragCombineItem>(DragCombineItemHandler);
		eventBrokerComponent.Subscribe<GameStateEvents.RestartGame>(RestartGameHandler);
	}

	~InventorySystem() 
    {
        eventBrokerComponent.Unsubscribe<InventoryEvents.AddItem>(AddItemHandler);
		eventBrokerComponent.Unsubscribe<InventoryEvents.RemoveItem>(RemoveItemHandler);
		eventBrokerComponent.Unsubscribe<InventoryEvents.HasItem>(HasItemHandler);
		eventBrokerComponent.Unsubscribe<InventoryEvents.SelectItem>(SelectItemHandler);
		eventBrokerComponent.Unsubscribe<InventoryEvents.DragCombineItem>(DragCombineItemHandler);
		eventBrokerComponent.Unsubscribe<GameStateEvents.RestartGame>(RestartGameHandler);
	}

    private void AddItemHandler(BrokerEvent<InventoryEvents.AddItem> inEvent)
    {
		foreach (InventoryItem item in inEvent.Payload.Items)
		{
            if (inventory.Contains(item)) continue;
            inventory.Add(item);
            if (item.ItemObtainedSFX != null)
                eventBrokerComponent.Publish(this, new AudioEvents.PlaySFX(item.ItemObtainedSFX));
        }
		
    }

	private void RemoveItemHandler(BrokerEvent<InventoryEvents.RemoveItem> inEvent)
	{
		foreach (InventoryItem item in inEvent.Payload.Items)
		{
            if (inventory.Contains(item))
            {
                inventory.Remove(item);
            }
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
		InventoryItem item1 = inEvent.Payload.Item1;
		InventoryItem item2 = inEvent.Payload.Item2;

		if (IsInvalidDragCombination(item1, item2))
		{
			invalidCombinationDialogue.Interact(this, Constants.Interaction.InteractionType.Virtual);
			return;
		}
        item1.InventoryInteraction.OnDragInteraction();
    }

    private bool IsInvalidDragCombination(InventoryItem item1, InventoryItem item2)
    {
		bool invalid = item1.InventoryInteraction == null || item2.InventoryInteraction == null;
		invalid = invalid || !item1.InventoryInteraction.RequiredItems.Contains(item2) || !item2.InventoryInteraction.RequiredItems.Contains(item1);

		return invalid;
    }

	private void RestartGameHandler(BrokerEvent<GameStateEvents.RestartGame> inEvent)
	{
		inventory = new HashSet<InventoryItem>();
		selectedItem = null;
	}

	//  private bool ContainsRequiredItems(InventoryItem item)
	//  {
	//return inventory.Contains(item.InventoryInteraction.RequiredItem);
	//  }
}
