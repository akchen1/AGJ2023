using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System;
using UnityEngine;
using UnityEngine.EventSystems;

public class Scene7Interaction : MonoBehaviour, IInteractable, IPointerClickHandler
{
	[SerializeField] private Constants.Scene7SubScenes subscene;
    [field: SerializeField] public FloatReference InteractionDistance { get; set; }
    [field: SerializeField] public bool HasInteractionDistance { get; set; } = false;

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
		eventBrokerComponent.Publish(this, new Scene7Events.ChangeSubscene(subscene));
	}

	public void OnPointerClick(PointerEventData eventData)
	{
		if (!active) return;
		Interact();
	}
}
