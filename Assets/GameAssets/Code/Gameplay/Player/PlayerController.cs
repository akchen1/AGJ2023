using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerController : MonoBehaviour
{
    [SerializeField] private Transform moveTarget;

    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();
    private Animator animator;
    private void OnEnable()
    {
        eventBrokerComponent.Subscribe<PlayerEvents.GetPlayerPosition>(GetPositionHandler);
        eventBrokerComponent.Subscribe<PlayerEvents.SetPlayerPosition>(SetPlayerPositionHandler);
    }

    private void OnDisable()
    {
        eventBrokerComponent.Unsubscribe<PlayerEvents.GetPlayerPosition>(GetPositionHandler);
        eventBrokerComponent.Unsubscribe<PlayerEvents.SetPlayerPosition>(SetPlayerPositionHandler);
    }

    private void SetPlayerPositionHandler(BrokerEvent<PlayerEvents.SetPlayerPosition> inEvent)
    {
        transform.position = inEvent.Payload.Position;
    }

    private void GetPositionHandler(BrokerEvent<PlayerEvents.GetPlayerPosition> inEvent)
    {
        inEvent.Payload.Position.DynamicInvoke(transform.position);
    }
}
