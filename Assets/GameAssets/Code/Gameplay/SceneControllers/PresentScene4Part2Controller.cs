using DS.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PresentScene4Part2Controller : MonoBehaviour
{
    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();
    [SerializeField] private DSDialogueSO startingDialogue;
    void Start()
    {
        eventBrokerComponent.Publish(this, new DialogueEvents.StartDialogue(startingDialogue));
    }
}
