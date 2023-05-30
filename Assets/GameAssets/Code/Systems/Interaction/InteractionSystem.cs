using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionSystem
{
    // Only one interaction event can occur at once.
    private IInteractable currentInteraction;

    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

    public InteractionSystem() 
    {
        eventBrokerComponent.Subscribe<InteractionEvents.Interact>(InteractHandler);
        eventBrokerComponent.Subscribe<InteractionEvents.InteractEnd>(InteractEndHandler);
        currentInteraction = null;
    }

    ~InteractionSystem()
    {
        eventBrokerComponent.Unsubscribe<InteractionEvents.Interact>(InteractHandler);
        eventBrokerComponent.Unsubscribe<InteractionEvents.InteractEnd>(InteractEndHandler);
    }

    private void InteractHandler(BrokerEvent<InteractionEvents.Interact> inEvent)
    {
        if (currentInteraction != null)
        {
            Debug.Log("Player is already interacting with an object");
            inEvent.Payload.Response?.Invoke(false);

            return;
        }
        currentInteraction = inEvent.Payload.Interactable;
        inEvent.Payload.Response?.Invoke(true);
    }

    private void InteractEndHandler(BrokerEvent<InteractionEvents.InteractEnd> obj)
    {
        currentInteraction = null;
    }

}
