using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class SceneChangeInteraction : MonoBehaviour, IInteractable, IPointerClickHandler
{
    [HideInInspector]
    public string selectedSceneName;

    [SerializeField] private bool unloadPrevious = true;
    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

    private bool active;

    private void OnEnable()
    {
        active = true;
    }

    private void OnDisable()
    {
        active = false;
    }

    public void Interact()
    {
        eventBrokerComponent.Publish(this, new InteractionEvents.Interact(this, valid =>
        {
            if (valid)
            {
                eventBrokerComponent.Publish(this, new SceneEvents.SceneChange(selectedSceneName, unloadPrevious));
            }
        }));
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!active) return;
        Interact();
    }
}
