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
    [Header("Clay minigame")]
    [SerializeField] private InventoryItem clayItem;
    [Tooltip("Dialogue to trigger after obtaining clay")]
    [SerializeField] private DSDialogueSO clayDialogue;
    [Tooltip("Decision node to either break sandcastle or not")]
    [SerializeField] private DSDialogueSO clayDecisionNode;
    [SerializeField] private GameObject sandCastle;

    private bool obtainedClay = false;
    private bool clayDialogueStarted = false;

    [Header("Ritual state")]
    [SerializeField] private List<CombinedRitualItem> combinedRitualItems;
    [Tooltip("Dialogue to trigger when all ritual components are obtained")]
    [SerializeField] private DSDialogueSO uncombinedDialogue;
    [Tooltip("Dialogue to trigger when ritual components are combined")]
    [SerializeField] private DSDialogueSO combinedDialogue;
    [SerializeField] private ScrollStateReference scrollStateReference;
    
    private bool componentsObtained;
    private bool itemsObtained;

    private static bool hasTriggeredComponentsObtainedDialogue = false;
    private static bool hasTriggeredItemsObtainedDialogue = false;

    public override void Enable()
    {
        base.Enable();
        eventBrokerComponent.Subscribe<InventoryEvents.AddItem>(AddItemHandler);
        eventBrokerComponent.Subscribe<InteractionEvents.InteractEnd>(InteractEndHandler);
        eventBrokerComponent.Subscribe<DialogueEvents.SelectDialogueOption>(SelectDialogueOptionHandler);
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
        eventBrokerComponent.Unsubscribe<DialogueEvents.SelectDialogueOption>(SelectDialogueOptionHandler);
    }

    private void SelectDialogueOptionHandler(BrokerEvent<DialogueEvents.SelectDialogueOption> obj)
    {
        if (!clayDialogueStarted) return;
        if (obj.Payload.Option == clayDecisionNode.Choices[0])
        {
            sandCastle.SetActive(false);
        }
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
        if (clayDialogueStarted)
        {
            clayDialogueStarted = false;
        }
        if (obtainedClay)
        {
            clayDialogueStarted = clayDialogue.Interact(this, Constants.Interaction.InteractionType.Virtual);
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
