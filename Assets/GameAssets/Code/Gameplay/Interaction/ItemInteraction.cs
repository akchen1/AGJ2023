using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ItemInteraction : MonoBehaviour, IInteractable
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
}
