using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CompositeInteraction : MonoBehaviour, IInteractableWorld, IPointerClickHandler
{
    [SerializeField] List<UnityEngine.Object> interactables;
    private int index = 0;
    private bool interacting = false;
    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

    [field: SerializeField] public bool HasInteractionDistance { get; set; }
    [field: SerializeField] public FloatReference InteractionDistance { get; set; }

    private void OnEnable()
    {
        eventBrokerComponent.Subscribe<InteractionEvents.InteractEnd>(InteractEndHandler);
    }

    private void OnDisable()
    {
        eventBrokerComponent.Unsubscribe<InteractionEvents.InteractEnd>(InteractEndHandler);
    }

    private void InteractEndHandler(BrokerEvent<InteractionEvents.InteractEnd> obj)
    {
        if (!interacting) return;
        index++;
        if (index >= interactables.Count)
        {
            interacting = false;
            return;
        }
        Interact();
    }

    public void Interact()
    {
        if (interactables[index] is IInteractableWorld interactable) {
            interactable.Interact();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        interacting = true;
        Interact();
    }
}
