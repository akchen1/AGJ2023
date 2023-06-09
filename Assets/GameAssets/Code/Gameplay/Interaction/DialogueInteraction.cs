using UnityEngine;
using DS.ScriptableObjects;
using System;
using UnityEngine.EventSystems;

public class DialogueInteraction : MonoBehaviour, IInteractable, IPointerClickHandler
{
    [SerializeField] private DSDialogueSO dialogue;
    private bool canInteract;
    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

    [field: SerializeField] public FloatReference InteractionDistance { get; set; }
    [field: SerializeField] public bool HasInteractionDistance { get; set; } = false;

    public void Interact()
    {
        if(canInteract){
            // Check first if there's another interaction event happening
            eventBrokerComponent.Publish(this, new InteractionEvents.Interact(this, (valid) =>
            {
                if (valid)
                {
                    eventBrokerComponent.Publish(this, new DialogueEvents.StartDialogue(dialogue));
                }
            }));
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Interact();
    }
    private void GetInputStateHandler(BrokerEvent<InputEvents.SetInputState> inEvent)
    {
        canInteract = inEvent.Payload.Active;
    }
    private void OnEnable() {
        eventBrokerComponent.Subscribe<InputEvents.SetInputState>(GetInputStateHandler);
    }
    private void OnDisable() {
        eventBrokerComponent.Unsubscribe<InputEvents.SetInputState>(GetInputStateHandler);
    }
}
