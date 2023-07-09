using System.Collections.Generic;

public static class InventoryUtility
{
    private static EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

    public static void RemoveFromInventory(this InventoryItem item, object sender)
    {
        eventBrokerComponent.Publish(sender, new InventoryEvents.RemoveItem(item));
    }

    public static void RemoveFromInventory(this List<InventoryItem> items, object sender)
    {
        items.ToArray().RemoveFromInventory(sender);
    }

    public static void RemoveFromInventory(this InventoryItem[] items, object sender)
    {
        eventBrokerComponent.Publish(sender, new InventoryEvents.RemoveItem(items));
    }

    public static void AddToInventory(this InventoryItem item, object sender)
    {
        eventBrokerComponent.Publish(sender, new InventoryEvents.AddItem(item));
    }

    public static void AddToInventory(this List<InventoryItem> items, object sender)
    {
        items.ToArray().AddToInventory(sender);
    }

    public static void AddToInventory(this InventoryItem[] items, object sender)
    {
        eventBrokerComponent.Publish(sender, new InventoryEvents.AddItem(items));
    }

    public static bool CheckInInventory(this InventoryItem item, object sender)
    {
        bool result = false;
        eventBrokerComponent.Publish(sender, new InventoryEvents.HasItem(contains =>
        {
            result = contains;
        }, item));
        return result;
    }

    public static bool CheckInInventory(this List<InventoryItem> items, object sender)
    {
        return items.ToArray().CheckInInventory(sender);
    }

    public static bool CheckInInventory(this InventoryItem[] items, object sender)
    {
        bool result = false;
        eventBrokerComponent.Publish(sender, new InventoryEvents.HasItem(contains =>
        {
            result = contains;
        }, items));
        return result;
    }
}
