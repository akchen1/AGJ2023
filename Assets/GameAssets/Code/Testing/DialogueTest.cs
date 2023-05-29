using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DS;
using DS.ScriptableObjects;

public class DialogueTest : MonoBehaviour
{
    [SerializeField] private DSDialogueSO startingDialogue;
    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();
    // Start is called before the first frame update
    void Start()
    {
        eventBrokerComponent.Publish(this, new DialogueEvents.StartDialogue(startingDialogue));
    }
}
