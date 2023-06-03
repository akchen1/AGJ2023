using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ItemInteraction : MonoBehaviour, IInteractable, IPointerClickHandler
{
    [SerializeField] private InventoryItem item;
    [SerializeField] private bool destroyOnInteract = false;

    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

    public void Interact()
    {
        eventBrokerComponent.Publish(this, new InventoryEvents.AddItem(item));
        if (destroyOnInteract)
            Destroy(this.gameObject);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Interact();
    }
}
