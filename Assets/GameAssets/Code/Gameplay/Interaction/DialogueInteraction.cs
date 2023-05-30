using UnityEngine;
using DS.ScriptableObjects;
using System;

public class DialogueInteraction : MonoBehaviour, IInteractable
{
    [SerializeField] private DSDialogueSO dialogue;

    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

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
        eventBrokerComponent.Publish(this, new InteractionEvents.InteractEnd());
    }

    public void Interact()
    {
        // Check first if there's another interaction event happening
        eventBrokerComponent.Publish(this, new InteractionEvents.Interact(this, (valid) =>
        {
            if (valid)
                eventBrokerComponent.Publish(this, new DialogueEvents.StartDialogue(dialogue));
        }));
    }
}
