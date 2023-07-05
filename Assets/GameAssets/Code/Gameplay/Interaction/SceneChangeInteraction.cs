using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.EventSystems;

public class SceneChangeInteraction : MonoBehaviour, IInteractableWorld, IPointerClickHandler
{
    [HideInInspector]
    public string selectedSceneName;

    [field: SerializeField] public FloatReference InteractionDistance { get; set; }
    [field: SerializeField] public bool HasInteractionDistance { get; set; } = false;


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
        if (gameObject.Interact())
        {
            eventBrokerComponent.Publish(this, new SceneEvents.SceneChange(selectedSceneName, unloadPrevious));

        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!active) return;
        Interact();
    }
}
