using DS.ScriptableObjects;
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


    [SerializeField] private List<CombinedRitualItem> combinedRitualItems;
    [SerializeField] private DSDialogueSO uncombinedDialogue;
    [SerializeField] private DSDialogueSO combinedDialogue;
    private bool hasComponents = false;
    private bool isCombined = false;
    
    public override void Enable()
    {
        base.Enable();
        eventBrokerComponent.Subscribe<InventoryEvents.AddItem>(AddItemHandler);
        eventBrokerComponent.Subscribe<InteractionEvents.InteractEnd>(InteractEndHandler);
        eventBrokerComponent.Subscribe<Scene7Events.HasCombinedItems>(HasCombinedItemsHandler);
        CheckEnterDialogues();
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
        eventBrokerComponent.Unsubscribe<Scene7Events.HasCombinedItems>(HasCombinedItemsHandler);

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

    private void HasCombinedItemsHandler(BrokerEvent<Scene7Events.HasCombinedItems> obj)
    {
        obj.Payload.Result?.Invoke(isCombined);
    }

    private void InteractEndHandler(BrokerEvent<InteractionEvents.InteractEnd> obj)
    {
        if (obtainedClay)
        {
            clayDialogue.Interact();
            obtainedClay = false;
        }
    }

    private void CheckEnterDialogues()
    {
        isCombined = true;
        hasComponents = true;
        foreach (CombinedRitualItem item in combinedRitualItems)
        {
            // Do we have main item
            eventBrokerComponent.Publish(this, new InventoryEvents.HasItem(has =>
            {
                // have main item go next
                if (has) return;
                isCombined = false; // dont have main item
                // Do we have componenets
                if (item.Components.Count == 0) return;

                eventBrokerComponent.Publish(this, new InventoryEvents.HasItem(has =>
                {
                    if (has) return; // we have components
                    hasComponents = false;

                }, item.Components.ToArray()));
            }, item.Item));

        }

        if (!isCombined && hasComponents)
        {
            eventBrokerComponent.Publish(this, new DialogueEvents.StartDialogue(uncombinedDialogue));
        } else if (isCombined)
        {
            eventBrokerComponent.Publish(this, new DialogueEvents.StartDialogue(combinedDialogue));
        }
    }

    [System.Serializable]
    public struct CombinedRitualItem
    {
        public InventoryItem Item;
        public List<InventoryItem> Components;
    }
}
