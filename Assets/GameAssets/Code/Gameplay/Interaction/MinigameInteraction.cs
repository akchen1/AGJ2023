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
    [SerializeField] protected bool canInteractMultipleTimes = false;
    protected int interactCount = 0;
    [field: SerializeField] public FloatReference InteractionDistance { get; set; }
    [field: SerializeField] public bool HasInteractionDistance { get; set; } = false;


    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    public void Interact()
    {
        if (!canInteractMultipleTimes && interactCount > 0) return;
        if (minigame.GetComponent<IMinigame>().Interact(this))
        {
            interactCount++;
        }
        if (!canInteractMultipleTimes && interactCount > 0)
        {
            gameObject.layer = 0;
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Interact();
    }
}
