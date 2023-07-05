using DS;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class MinigameInteraction : MonoBehaviour, IInteractableWorld, IPointerClickHandler
{
    [SerializeField] private GameObject minigame;

    [field: SerializeField] public FloatReference InteractionDistance { get; set; }
    [field: SerializeField] public bool HasInteractionDistance { get; set; } = false;


    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

    public void Interact()
    {
        minigame.GetComponent<IMinigame>().Interact(this);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Interact();
    }
}
