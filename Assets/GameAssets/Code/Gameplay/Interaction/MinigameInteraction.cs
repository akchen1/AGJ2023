using DS;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;
using UnityEngine.Rendering;

public class MinigameInteraction : MonoBehaviour, IInteractableWorld, IPointerClickHandler
{
    [SerializeField] private GameObject minigame;
    [SerializeField] protected bool canInteractMultipleTimes = false;
    protected int interactCount = 0;
    [field: SerializeField] public FloatReference InteractionDistance { get; set; }
    [field: SerializeField] public bool HasInteractionDistance { get; set; } = false;

    public UnityEvent onMinigameStart;
    public UnityEvent onMinigameFinish;

    private bool started = false;

    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

    private void OnEnable()
    {
        eventBrokerComponent.Subscribe<MinigameEvents.EndMinigame>(EndMinigameHandler);
    }

    private void OnDisable()
    {
        eventBrokerComponent.Unsubscribe<MinigameEvents.EndMinigame>(EndMinigameHandler);
    }

    private void EndMinigameHandler(BrokerEvent<MinigameEvents.EndMinigame> obj)
    {
        if (!started) return;
        started = false;
        onMinigameFinish?.Invoke();
    }

    public void Interact()
    {
        if (!canInteractMultipleTimes && interactCount > 0) return;
        if (minigame.GetComponent<IMinigame>().Interact(this))
        {
            interactCount++;
            started = true;
            onMinigameStart?.Invoke();
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
