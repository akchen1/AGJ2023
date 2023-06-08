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
    }

    private void GetPositionHandler(BrokerEvent<PlayerEvents.GetPlayerPosition> inEvent)
    {
        inEvent.Payload.Position.DynamicInvoke(transform.position);
    }

    private void OnDisable()
    {
        eventBrokerComponent.Unsubscribe<PlayerEvents.GetPlayerPosition>(GetPositionHandler);
    }
}
