using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MinigameUtility
{
    static EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();
    public static bool CheckIfHasAllRequiredItems(this List<InventoryItem> requiredItems, object sender)
    {
        foreach (InventoryItem item in requiredItems)
        {
            bool hasItem = true;
            eventBrokerComponent.Publish(sender, new InventoryEvents.HasItem(item, callback =>
            {
                hasItem = callback;
            }));
            if (!hasItem)
            {
                return false;
            }
        }
        return true;
    }
}
