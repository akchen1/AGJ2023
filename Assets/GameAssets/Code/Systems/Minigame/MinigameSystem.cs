using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameSystem : MonoBehaviour
{
    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

    private IMinigame activeMinigame;

    private void OnEnable()
    {
        eventBrokerComponent.Subscribe<MinigameEvents.StartMinigame>(StartMinigameHandler);
        eventBrokerComponent.Subscribe<MinigameEvents.EndMinigame>(EndMinigameHandler);
    }

    private void OnDisable()
    {
        eventBrokerComponent.Unsubscribe<MinigameEvents.StartMinigame>(StartMinigameHandler);
        eventBrokerComponent.Unsubscribe<MinigameEvents.EndMinigame>(EndMinigameHandler);
    }

    private void StartMinigameHandler(BrokerEvent<MinigameEvents.StartMinigame> inEvent)
    {
        if (activeMinigame != null)
        {
            Debug.LogError("A minigame is already going on");
            return;
        }
        eventBrokerComponent.Publish(this, new InputEvents.SetInputState(false));
        activeMinigame = inEvent.Payload.Minigame;
        
        activeMinigame.Initialize();
    }

    private void EndMinigameHandler(BrokerEvent<MinigameEvents.EndMinigame> obj)
    {
        if (activeMinigame == null)
        {
            Debug.LogError("Unable to end minigame that has not been started");
            return;
        }
        eventBrokerComponent.Publish(this, new InputEvents.SetInputState(true));

        //activeMinigame.Finish();
        activeMinigame = null;
        eventBrokerComponent.Publish(this, new InteractionEvents.InteractEnd());
    }
}
