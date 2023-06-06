using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();
    private Animator animator;
    private void OnEnable()
    {
        eventBrokerComponent.Subscribe<PlayerEvents.GetPlayerPosition>(GetPositionHandler);
        //eventBrokerComponent.Subscribe<InputEvents.MouseClick>(MouseClickHandler);
    }

    private void GetPositionHandler(BrokerEvent<PlayerEvents.GetPlayerPosition> inEvent)
    {
        inEvent.Payload.Position.DynamicInvoke(transform.position);
    }

    private void OnDisable()
    {
        eventBrokerComponent.Unsubscribe<PlayerEvents.GetPlayerPosition>(GetPositionHandler);
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
