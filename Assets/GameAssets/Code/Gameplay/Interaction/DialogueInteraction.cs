using UnityEngine;
using DS.ScriptableObjects;
using System;
using UnityEngine.EventSystems;

public class DialogueInteraction : MonoBehaviour, IInteractableWorld, IPointerClickHandler
{
    [SerializeField] private DSDialogueSO dialogue;
    [SerializeField] private bool canInteractMultipleTimes = true;
    private int interactCount = 0;
    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

    [field: SerializeField] public FloatReference InteractionDistance { get; set; }
    [field: SerializeField] public bool HasInteractionDistance { get; set; } = false;

    public void Interact()
    {
        if (!canInteractMultipleTimes && interactCount > 0) return;
        // Check first if there's another interaction event happening
        if (dialogue.Interact(this))
        {
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
