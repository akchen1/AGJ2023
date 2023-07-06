using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class InteractionSystem
{
    // Only one interaction event can occur at once.
    private UnityEngine.Object currentInteraction;

    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

    private Coroutine waitCoroutine = null;

    private Bootstrap bootstrap;

    public InteractionSystem(Bootstrap bootstrap) 
    {
        eventBrokerComponent.Subscribe<InteractionEvents.Interact>(InteractHandler);
        eventBrokerComponent.Subscribe<InteractionEvents.InteractEnd>(InteractEndHandler);
        eventBrokerComponent.Subscribe<InteractionEvents.CancelPendingInteraction>(CancelPendingInteractionHandler);
        currentInteraction = null;
        this.bootstrap = bootstrap;
    }

    ~InteractionSystem()
    {
        eventBrokerComponent.Unsubscribe<InteractionEvents.Interact>(InteractHandler);
        eventBrokerComponent.Unsubscribe<InteractionEvents.InteractEnd>(InteractEndHandler);
        eventBrokerComponent.Unsubscribe<InteractionEvents.CancelPendingInteraction>(CancelPendingInteractionHandler);
    }

    private void InteractHandler(BrokerEvent<InteractionEvents.Interact> inEvent)
    {
        if (currentInteraction != null)
        {
            inEvent.Payload.Response?.Invoke(false);

            return;
        }

        if (inEvent.Payload.InteractType == Constants.Interaction.InteractionType.Virtual || CheckInRange(inEvent))
        {
            StartInteraction(inEvent);
        } else
        {
            if (waitCoroutine != null)
            {
                bootstrap.StopCoroutine(waitCoroutine);
            }
            waitCoroutine = bootstrap.StartCoroutine(WaitForInRange(inEvent));
        }

    }

    private void CancelPendingInteractionHandler(BrokerEvent<InteractionEvents.CancelPendingInteraction> obj)
    {
        if (waitCoroutine != null)
        {
            bootstrap.StopCoroutine(waitCoroutine);
            waitCoroutine = null;
        }
    }

    private void InteractEndHandler(BrokerEvent<InteractionEvents.InteractEnd> obj)
    {
        currentInteraction = null;
    }

    private void StartInteraction(BrokerEvent<InteractionEvents.Interact> inEvent)
    {
        eventBrokerComponent.Publish(this, new InventoryEvents.ToggleInventoryVisibility(false));
        currentInteraction = inEvent.Payload.Interactable;
        inEvent.Payload.Response?.Invoke(true);
    }
    
    private bool CheckInRange(BrokerEvent<InteractionEvents.Interact> inEvent)
    {
        IInteractableWorld worldInteraction = inEvent.Payload.Interactable.GetComponent<IInteractableWorld>();
        if (!worldInteraction.HasInteractionDistance) return true;
        bool inRange = false;
        eventBrokerComponent.Publish(this, new PlayerEvents.GetPlayerPosition(position =>
        {
            Collider2D collider = ((UnityEngine.Object)inEvent.Sender).GetComponent<Collider2D>();
            float distance = (collider.bounds.center - position).magnitude;
            inRange = distance <= worldInteraction.InteractionDistance;
        }));
        return inRange;
    }

    private IEnumerator WaitForInRange(BrokerEvent<InteractionEvents.Interact> inEvent)
    {
        yield return new WaitUntil(() => CheckInRange(inEvent));
        StartInteraction(inEvent);
    }

}
