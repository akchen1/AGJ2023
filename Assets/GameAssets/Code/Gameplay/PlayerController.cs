using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();
    private void OnEnable()
    {
        //eventBrokerComponent.Subscribe<InputEvents.MouseClick>(MouseClickHandler);
    }

    private void OnDisable()
    {
        //eventBrokerComponent.Unsubscribe<InputEvents.MouseClick>(MouseClickHandler);
    }

    private void MouseClickHandler(BrokerEvent<InputEvents.MouseClick> inEvent)
    {
        //LayerMask interactablesLayer = LayerMask.NameToLayer("Interactables");
        //Collider2D hit = Physics2D.OverlapCircle(inEvent.Payload.MousePosition.ScreenToWorldPoint(), 0.1f, 1 << interactablesLayer.value);
        
        //if (hit == null) return;
        //IInteractable interactable = hit.GetComponent<IInteractable>();
        //if (interactable == null) return;
        //Debug.Log(hit.name);
        //interactable.Interact();
    }
}
