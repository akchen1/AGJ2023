using UnityEngine;
using DS.ScriptableObjects;
using System;
using UnityEngine.EventSystems;

public class DialogueInteraction : MonoBehaviour, IInteractable, IPointerClickHandler
{
    [SerializeField] private DSDialogueSO dialogue;

    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

    private bool active;

    private void OnEnable()
    {
        eventBrokerComponent.Subscribe<DialogueEvents.DialogueFinish>(DialogueFinishHandler);
    }

    private void OnDisable()
    {
        eventBrokerComponent.Unsubscribe<DialogueEvents.DialogueFinish>(DialogueFinishHandler);
    }

    private void DialogueFinishHandler(BrokerEvent<DialogueEvents.DialogueFinish> obj)
    {
        if (!active) return;
        eventBrokerComponent.Publish(this, new InteractionEvents.InteractEnd());
    }

    public void Interact()
    {
        // Check first if there's another interaction event happening
        eventBrokerComponent.Publish(this, new InteractionEvents.Interact(this, (valid) =>
        {
            if (valid)
            {
                eventBrokerComponent.Publish(this, new DialogueEvents.StartDialogue(dialogue));
                active = true;
            }
        }));
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Interact();
    }
}
