using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTarget : MonoBehaviour
{
    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

    private GameObject hitObject = null;

    private TargetMover targetMover;

    private void Awake()
    {
        targetMover = GetComponent<TargetMover>();
    }

    void Start()
    {
        eventBrokerComponent.Publish(this, new PlayerEvents.GetPlayerPosition((position) => 
        {
            transform.position = position;
        }));
        hitObject = targetMover.hitGameObject;
    }

    private void Update()
    {
        if (hitObject != null && targetMover.hitGameObject == null)
        {
            eventBrokerComponent.Publish(this, new InteractionEvents.CancelPendingInteraction());
            hitObject = null;
        }

        if (targetMover.hitGameObject != null && targetMover.hitGameObject.GetComponent<IInteractable>() != null)
        {
            hitObject = targetMover.hitGameObject;
        }
    }

    private void GetInputStateHandler(BrokerEvent<InputEvents.SetInputState> inEvent)
    {
        targetMover.AllowMove = inEvent.Payload.Active;
    }
    private void OnEnable() {
        eventBrokerComponent.Subscribe<InputEvents.SetInputState>(GetInputStateHandler);
        eventBrokerComponent.Subscribe<PlayerEvents.SetPlayerPosition>(SetPlayerPositionHandler);
    }

    private void OnDisable() {
        eventBrokerComponent.Unsubscribe<InputEvents.SetInputState>(GetInputStateHandler);
        eventBrokerComponent.Unsubscribe<PlayerEvents.SetPlayerPosition>(SetPlayerPositionHandler);
    }
    private void SetPlayerPositionHandler(BrokerEvent<PlayerEvents.SetPlayerPosition> obj)
    {
        transform.position = obj.Payload.Position;
    }

}
