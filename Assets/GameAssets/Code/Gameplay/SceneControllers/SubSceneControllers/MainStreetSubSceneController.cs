using Cinemachine;
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
    private bool componentsObtained;
    private bool itemsObtained;
    [SerializeField] private ScrollStateReference scrollStateReference;
    private static bool hasTriggeredComponentsObtainedDialogue = false;
    private static bool hasTriggeredItemsObtainedDialogue = false;
    public override void Enable()
    {
        base.Enable();
        eventBrokerComponent.Subscribe<InventoryEvents.AddItem>(AddItemHandler);
        eventBrokerComponent.Subscribe<InteractionEvents.InteractEnd>(InteractEndHandler);
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

    

    private void CheckEnterDialogues()
    {
        itemsObtained = true;
        componentsObtained = true;
        foreach (CombinedRitualItem item in combinedRitualItems)
        {
            if (item.Item.CheckInInventory(this)) continue;
            itemsObtained = false;
            if (item.Components.Count > 0 && item.Components.CheckInInventory(this)) continue;
            componentsObtained = false;
        }
        if (itemsObtained && !hasTriggeredItemsObtainedDialogue)
        {
            scrollStateReference.Variable.SetValue(ScrollState.ItemsObtained);
            hasTriggeredItemsObtainedDialogue = true;
            combinedDialogue.Interact(this, Constants.Interaction.InteractionType.Virtual);
        } else if (!itemsObtained && componentsObtained && !hasTriggeredComponentsObtainedDialogue)
        {
            scrollStateReference.Variable.SetValue(ScrollState.ComponentsObtained);
            hasTriggeredComponentsObtainedDialogue = true;
            uncombinedDialogue.Interact(this, Constants.Interaction.InteractionType.Virtual);
        }
    }

    [System.Serializable]
    public struct CombinedRitualItem
    {
        public InventoryItem Item;
        public List<InventoryItem> Components;
    }
}
