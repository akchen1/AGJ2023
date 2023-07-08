using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.EventSystems;

public class EmptyInteraction : MonoBehaviour, IInteractableWorld, IPointerClickHandler
{
    [field: SerializeField] public bool HasInteractionDistance { get; set; }
    [field: SerializeField] public FloatReference InteractionDistance { get; set; }

    [SerializeField] private UnityEvent interactionStart;

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }

    public void Interact()
    {
        if (gameObject.Interact())
        {
            interactionStart?.Invoke();
            gameObject.EndInteract();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Interact();
    }
}
