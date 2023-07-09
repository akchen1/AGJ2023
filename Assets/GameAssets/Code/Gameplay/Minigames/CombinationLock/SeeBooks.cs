using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class SeeBooks : MonoBehaviour, IPointerClickHandler, IInteractableWorld
{
    [SerializeField] GameObject panel;
    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

    [field: SerializeField] public bool HasInteractionDistance { get; set; } = false;
    [field: SerializeField] public FloatReference InteractionDistance { get; set; }

    public void CloseBooks()
    {
        eventBrokerComponent.Publish(this, new InteractionEvents.InteractEnd());
        panel.SetActive(false);
    }

    public void Interact()
    {
        panel.SetActive(true);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (gameObject.Interact())
        {
            Interact();
        }
    }
}
