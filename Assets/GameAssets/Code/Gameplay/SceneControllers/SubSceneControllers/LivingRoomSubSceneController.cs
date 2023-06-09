using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class LivingRoomSubSceneController : SubSceneController
{
    [SerializeField] private InventoryItem pocketKnife;
    [SerializeField] private DialogueInteraction onKnifeObtainedDialogue;

    private bool pocketKnifeObtained = false;
    public override void Enable()
    {
        base.Enable();
        eventBrokerComponent.Subscribe<InventoryEvents.AddItem>(AddItemHandler);
        eventBrokerComponent.Subscribe<InteractionEvents.InteractEnd>(InteractEndHandler);
    }

    public override void Disable()
    {
        base.Disable();
        eventBrokerComponent.Unsubscribe<InventoryEvents.AddItem>(AddItemHandler);
        eventBrokerComponent.Unsubscribe<InteractionEvents.InteractEnd>(InteractEndHandler);
    }

    private void AddItemHandler(BrokerEvent<InventoryEvents.AddItem> obj)
    {
        foreach (InventoryItem item in obj.Payload.Items)
        {
            if (item == pocketKnife)
            {
                pocketKnifeObtained = true;
            }
        }
    }

    private void InteractEndHandler(BrokerEvent<InteractionEvents.InteractEnd> obj)
    {
        if (pocketKnifeObtained)
        {
            onKnifeObtainedDialogue.Interact();
            pocketKnifeObtained = false;
        }
    }
}
