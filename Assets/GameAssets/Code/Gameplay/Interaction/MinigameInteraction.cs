using DS;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class MinigameInteraction : MonoBehaviour, IInteractable, IPointerClickHandler
{
    [SerializeField] private GameObject minigame;

    [field: SerializeField] public FloatReference InteractionDistance { get; set; }
    [field: SerializeField] public bool HasInteractionDistance { get; set; } = false;


    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

    public void Interact()
    {
        IMinigame iMinigame = minigame.GetComponent<IMinigame>();
        // Check if conditions are met
        if (!iMinigame.StartCondition()) return;
        
        // Check if there's another interaction event happening
        eventBrokerComponent.Publish(this, new InteractionEvents.Interact(this, (valid) =>
        {
            if (valid)
            {
                eventBrokerComponent.Publish(this, new MinigameEvents.StartMinigame(minigame.GetComponent<IMinigame>()));
            }
        }));
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Interact();
    }
}
