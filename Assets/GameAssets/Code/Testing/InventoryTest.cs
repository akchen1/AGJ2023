using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryTest : MonoBehaviour
{
    EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();
    public List<InventoryItem> inventoryItems;
    void Start()
    {
        for (int i = 0; i < inventoryItems.Count; i++)
        {
            InventoryItem inventoryItem = inventoryItems[i];
            //inventoryItem.ItemDescription = "Test item " + i;
            eventBrokerComponent.Publish(this, new InventoryEvents.AddItem(inventoryItem));
        }
    }
}
