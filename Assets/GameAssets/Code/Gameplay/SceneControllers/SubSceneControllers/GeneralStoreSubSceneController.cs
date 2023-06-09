using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class GeneralStoreSubSceneController : SubSceneController
{
    [SerializeField] private InventoryInteraction matches;
    [SerializeField] private InventoryInteraction twine;
    public override void Enable()
    {
        base.Enable();
        eventBrokerComponent.Subscribe<MinigameEvents.EndMinigame>(EndMinigameHandler);
    }

    public override void Disable() 
    { 
        base.Disable();
        eventBrokerComponent.Unsubscribe<MinigameEvents.EndMinigame>(EndMinigameHandler);
    }

    private void EndMinigameHandler(BrokerEvent<MinigameEvents.EndMinigame> obj)
    {
        matches.InteractionDistance.ConstantValue = 4f;
    }
}
