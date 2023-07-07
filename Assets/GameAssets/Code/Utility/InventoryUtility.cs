using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;

public static class InventoryUtility
{
    private static EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

    public static void RemoveFromInventory(this InventoryItem item, Object sender)
    {
        eventBrokerComponent.Publish(sender, new InventoryEvents.RemoveItem(item));
    }

    public static void RemoveFromInventory(this List<InventoryItem> items, Object sender)
    {
        items.ToArray().RemoveFromInventory(sender);
    }

    public static void RemoveFromInventory(this InventoryItem[] items, Object sender)
    {
        eventBrokerComponent.Publish(sender, new InventoryEvents.RemoveItem(items));
    }

    public static void AddToInventory(this List<InventoryItem> items, Object sender)
    {
        items.ToArray().AddToInventory(sender);
    }

    public static void AddToInventory(this InventoryItem[] items, Object sender)
    {
        eventBrokerComponent.Publish(sender, new InventoryEvents.AddItem(items));
    }

    public static bool CheckInInventory(this List<InventoryItem> items, Object sender)
    {
        return items.ToArray().CheckInInventory(sender);
    }

    public static bool CheckInInventory(this InventoryItem[] items, Object sender)
    {
        bool result = false;
        eventBrokerComponent.Publish(sender, new InventoryEvents.HasItem(contains =>
        {
            result = contains;
        }, items));
        return result;
    }
}
