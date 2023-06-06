using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MoveTarget : MonoBehaviour
{
    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

    // Start is called before the first frame update
    void Start()
    {
        eventBrokerComponent.Publish(this, new PlayerEvents.GetPlayerPosition((position) => 
        {
            transform.position = position;
        }));
    }
    private void GetInputStateHandler(BrokerEvent<InputEvents.SetInputState> inEvent)
    {
        GetComponent<Pathfinding.TargetMover>().AllowMove = inEvent.Payload.Active;
    }
    private void OnEnable() {
        eventBrokerComponent.Subscribe<InputEvents.SetInputState>(GetInputStateHandler);
    }
    private void OnDisable() {
        eventBrokerComponent.Unsubscribe<InputEvents.SetInputState>(GetInputStateHandler);
    }
}
