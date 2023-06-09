using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class MainStreetSubSceneController : SubSceneController
{
    [SerializeField] private InventoryItem clayItem;
    [SerializeField] private DialogueInteraction clayDialogue;
    private bool obtainedClay = false;
    public override void Enable()
    {
        base.Enable();
        eventBrokerComponent.Subscribe<InventoryEvents.AddItem>(AddItemHandler);
        eventBrokerComponent.Subscribe<InteractionEvents.InteractEnd>(InteractEndHandler);
    }

    

    public override void Disable()
    {
        base.Disable();
        eventBrokerComponent.Publish(this, new PlayerEvents.GetPlayerPosition(position =>
        {
            subsceneTeleportMarker.position = position;
        }));
        eventBrokerComponent.Unsubscribe<InventoryEvents.AddItem>(AddItemHandler);
        eventBrokerComponent.Unsubscribe<InteractionEvents.InteractEnd>(InteractEndHandler);
    }

    private void AddItemHandler(BrokerEvent<InventoryEvents.AddItem> obj)
    {
        foreach (InventoryItem item in obj.Payload.Items)
        {
            if (item == clayItem)
            {
                obtainedClay = true;
                return;
            }
        }
    }

    private void InteractEndHandler(BrokerEvent<InteractionEvents.InteractEnd> obj)
    {
        if (obtainedClay)
        {
            clayDialogue.Interact();
            obtainedClay = false;
        }
    }
}
