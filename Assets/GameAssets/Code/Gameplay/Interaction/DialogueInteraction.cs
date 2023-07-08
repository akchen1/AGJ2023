using UnityEngine;
using DS.ScriptableObjects;
using System;
using UnityEngine.EventSystems;

public class DialogueInteraction : MonoBehaviour, IInteractableWorld, IPointerClickHandler
{
    [SerializeField] protected DSDialogueSO dialogue;
    [SerializeField] protected bool canInteractMultipleTimes = true;
    [SerializeField] protected bool destroyOnDialogueFinish = false;
    protected int interactCount = 0;
    protected EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

    [field: SerializeField] public FloatReference InteractionDistance { get; set; }
    [field: SerializeField] public bool HasInteractionDistance { get; set; } = false;

    protected bool isInteracting;

    protected virtual void OnEnable()
    {
        eventBrokerComponent.Subscribe<DialogueEvents.DialogueFinish>(DialogueFinishHandler);
    }

    protected virtual void OnDisable()
    {
        eventBrokerComponent.Unsubscribe<DialogueEvents.DialogueFinish>(DialogueFinishHandler);
    }

    protected virtual void DialogueFinishHandler(BrokerEvent<DialogueEvents.DialogueFinish> obj)
    {
        if (isInteracting && destroyOnDialogueFinish)
        {
            Destroy(gameObject);
            return;
        }
        isInteracting = false;
    }

    public void Interact()
    {
        if (!canInteractMultipleTimes && interactCount > 0) return;
        // Check first if there's another interaction event happening
        if (dialogue.Interact(this))
        {
            isInteracting = true;
            interactCount++;
        }
        if (!canInteractMultipleTimes && interactCount > 0)
        {
            gameObject.layer = 0;
        }

    }
    public void OnPointerClick(PointerEventData eventData)
    {
        Interact();
    }

}
