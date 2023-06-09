using DS.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemInteraction : MonoBehaviour, IInteractable, IPointerClickHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private InventoryItem item;
    [SerializeField] private bool destroyOnInteract = false;
    [SerializeField] private bool allowDragClick = false;
    [SerializeField] private bool mustBeInRange = false;

    [SerializeField] private DSDialogueSO itemObtainedDialogue;

    private bool dragging = false;

    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

    [field: SerializeField] public FloatReference InteractionDistance { get; set; }

    public void Interact()
    {
        eventBrokerComponent.Publish(this, new InventoryEvents.AddItem(item));
        if (itemObtainedDialogue != null)
        {
            eventBrokerComponent.Publish(this, new DialogueEvents.StartDialogue(itemObtainedDialogue));
        }
        if (mustBeInRange)
        {
            eventBrokerComponent.Publish(this, new InteractionEvents.InteractEnd());
        }
        if (destroyOnInteract)
            Destroy(this.gameObject);
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        dragging = true;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        dragging = false;
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!allowDragClick && dragging) return;
        if (mustBeInRange)
        {
            eventBrokerComponent.Publish(this, new InteractionEvents.Interact(this, valid =>
            {
                if (valid)
                {
                    Interact();
                }
            }));
        } else
        {
            Interact();
        }
    }
}
