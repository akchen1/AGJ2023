using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class InventorySystem {


    public InventorySystem() 
    {
        Bootstrap.EventBrokerComponent.Subscribe<Event.AddItem>(AddItemHandler);
    }
    ~InventorySystem() 
    {
        Bootstrap.EventBrokerComponent.Unsubscribe<Event.AddItem>(AddItemHandler);
    }

    private void AddItemHandler(BrokerEvent<Event.AddItem> obj)
    {
        throw new NotImplementedException();
    }


    
}
