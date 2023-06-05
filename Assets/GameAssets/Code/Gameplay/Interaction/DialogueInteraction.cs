using UnityEngine;
using DS.ScriptableObjects;
using System;
using UnityEngine.EventSystems;

public class DialogueInteraction : MonoBehaviour, IInteractable, IPointerClickHandler
{
    [SerializeField] private DSDialogueSO dialogue;

    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

    public void Interact()
    {
        // Check first if there's another interaction event happening
        eventBrokerComponent.Publish(this, new InteractionEvents.Interact(this, (valid) =>
        {
            if (valid)
            {
                eventBrokerComponent.Publish(this, new DialogueEvents.StartDialogue(dialogue));
            }
        }));
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Interact();
    }
}
