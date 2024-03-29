using DS.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemInteraction : MonoBehaviour, IInteractableWorld, IPointerClickHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private InventoryItem item;
	[SerializeField] private bool destroyOnInteract = false;
    [SerializeField] private bool allowDragClick = false;
    [SerializeField] private bool ignoreInteractionSystem = false;

    [SerializeField] private DSDialogueSO itemObtainedDialogue;

    private bool dragging = false;

    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

    [field: SerializeField] public FloatReference InteractionDistance { get; set; }
    [field: SerializeField] public bool HasInteractionDistance { get; set; } = false;

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    public void Interact()
    {
        eventBrokerComponent.Publish(this, new InventoryEvents.AddItem(item));


        if (itemObtainedDialogue != null)
            eventBrokerComponent.Publish(this, new DialogueEvents.StartDialogue(itemObtainedDialogue));
        else if (!ignoreInteractionSystem)
            eventBrokerComponent.Publish(this, new InteractionEvents.InteractEnd());


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
        if (ignoreInteractionSystem || gameObject.Interact())
        {
            Interact();
        }
    }
}
