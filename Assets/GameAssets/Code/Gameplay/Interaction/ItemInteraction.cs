using DS.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemInteraction : MonoBehaviour, IInteractable, IPointerClickHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private InventoryItem item;
	[SerializeField] public string PickupAudio;
	[SerializeField] private bool destroyOnInteract = false;
    [SerializeField] private bool allowDragClick = false;

    [SerializeField] private DSDialogueSO itemObtainedDialogue;

    private bool dragging = false;

    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

    [field: SerializeField] public FloatReference InteractionDistance { get; set; }
    [field: SerializeField] public bool HasInteractionDistance { get; set; } = false;

    public void Interact()
    {
		if (PickupAudio != null)
		{
			eventBrokerComponent.Publish(this, new AudioEvents.PlaySFX(PickupAudio));
		}

        eventBrokerComponent.Publish(this, new InventoryEvents.AddItem(item));
        if (itemObtainedDialogue != null)
        {
            eventBrokerComponent.Publish(this, new DialogueEvents.StartDialogue(itemObtainedDialogue));
        }
        if (HasInteractionDistance)
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
        if (HasInteractionDistance)
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
