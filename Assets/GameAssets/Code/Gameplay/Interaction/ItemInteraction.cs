using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemInteraction : MonoBehaviour, IInteractable, IPointerClickHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private InventoryItem item;
    [SerializeField] private bool destroyOnInteract = false;
    [SerializeField] private bool allowDragClick = false;

    private bool dragging = false;

    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

    public void Interact()
    {
        eventBrokerComponent.Publish(this, new InventoryEvents.AddItem(item));
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
        Interact();
    }
}
