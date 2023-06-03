using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

public class PresentScene4Part1Controller : MonoBehaviour
{
    [SerializeField] private UnityEngine.Object badu;
    [SerializeField] private GameObject bedroomDoor;

    EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

    private void OnEnable()
    {
        eventBrokerComponent.Subscribe<InteractionEvents.InteractEnd>(InteractEndHandler);
    }

    

    private void OnDisable()
    {
        eventBrokerComponent.Unsubscribe<InteractionEvents.InteractEnd>(InteractEndHandler);
    }

    private void InteractEndHandler(BrokerEvent<InteractionEvents.InteractEnd> inEvent)
    {
        UnityEngine.Object sender = (UnityEngine.Object)inEvent.Sender;
        if (sender == badu.GetComponent<DialogueInteraction>())
        {
            bedroomDoor.GetComponent<DialogueInteraction>().enabled = false;
            bedroomDoor.GetComponent<SceneChangeInteraction>().enabled = true;
        }
    }
}
