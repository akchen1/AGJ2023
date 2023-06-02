using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InventoryTest : MonoBehaviour
{
    EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

    void Start()
    {
        for (int i = 0; i < 10; i++)
        {
            InventoryItem inventoryItem = ScriptableObject.CreateInstance<InventoryItem>();
            //inventoryItem.ItemDescription = "Test item " + i;
            eventBrokerComponent.Publish(this, new InventoryEvents.AddItem(inventoryItem));
        }
    }
}
