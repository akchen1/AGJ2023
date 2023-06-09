using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using static UnityEngine.Rendering.VolumeComponent;

[System.Serializable]
public class InteractionSystem
{
    // Only one interaction event can occur at once.
    private IInteractable currentInteraction;

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
            Debug.Log("Player is already interacting with an object " + currentInteraction.ToString());
            inEvent.Payload.Response?.Invoke(false);

            return;
        }

        if (CheckInRange(inEvent))
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
        Debug.Log("interaction over " + obj.Sender.ToString());
    }

    private void StartInteraction(BrokerEvent<InteractionEvents.Interact> inEvent)
    {
        Debug.Log("interacting with " + inEvent.Sender.ToString());
        eventBrokerComponent.Publish(this, new InventoryEvents.ToggleInventoryVisibility(false));
        currentInteraction = inEvent.Payload.Interactable;
        inEvent.Payload.Response?.Invoke(true);
    }
    
    private bool CheckInRange(BrokerEvent<InteractionEvents.Interact> inEvent)
    {
        if (!inEvent.Payload.Interactable.HasInteractionDistance) return true;
        bool inRange = false;
        eventBrokerComponent.Publish(this, new PlayerEvents.GetPlayerPosition(position =>
        {
            float distance = (((UnityEngine.Object)inEvent.Sender).GetComponent<Transform>().position - position).magnitude;
            inRange = distance <= inEvent.Payload.Interactable.InteractionDistance;
        }));
        return inRange;
    }

    private IEnumerator WaitForInRange(BrokerEvent<InteractionEvents.Interact> inEvent)
    {
        yield return new WaitUntil(() => CheckInRange(inEvent));
        StartInteraction(inEvent);
    }

}
