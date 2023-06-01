using DS;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;

public class MinigameInteraction : MonoBehaviour, IInteractable
{
    [SerializeField] private GameObject minigame;

    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

    public void Interact()
    {
        // Check first if there's another interaction event happening
        eventBrokerComponent.Publish(this, new InteractionEvents.Interact(this, (valid) =>
        {
            if (valid)
            {
                eventBrokerComponent.Publish(this, new MinigameEvents.StartMinigame(minigame.GetComponent<IMinigame>()));
            }
        }));
    }

}
