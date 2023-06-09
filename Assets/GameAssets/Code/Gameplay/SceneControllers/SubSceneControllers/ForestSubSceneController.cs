using DS.ScriptableObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ForestSubSceneController : SubSceneController
{
    [SerializeField] private DSDialogueSO sanityDialogue;
    [SerializeField] private GameObject branchBundle;

    [SerializeField] DSDialogueSO allComponentsDialogue;
    [SerializeField] GameObject clearingSceneInteraction;

    private bool isSanityDialogue = false;
    
    public override void Enable()
    {
        base.Enable();
        eventBrokerComponent.Subscribe<DialogueEvents.StartDialogue>(StartDialogueHandler);
        eventBrokerComponent.Subscribe<DialogueEvents.DialogueFinish>(DialogueFinishHandler);
        Debug.Log("h");
        eventBrokerComponent.Publish(this, new Scene7Events.HasCombinedItems(result =>
        {
            Debug.Log(result);
            if (result)
            {
                eventBrokerComponent.Publish(this, new DialogueEvents.StartDialogue(allComponentsDialogue));
                clearingSceneInteraction.SetActive(true);
            };
        }));
    }

    private void DialogueFinishHandler(BrokerEvent<DialogueEvents.DialogueFinish> inEvent)
    {
        if (isSanityDialogue)
        {
            branchBundle.SetActive(true);
            isSanityDialogue = false;
        }
    }

    private void StartDialogueHandler(BrokerEvent<DialogueEvents.StartDialogue> inEvent)
    {
        isSanityDialogue = inEvent.Payload.StartingDialogue == sanityDialogue;
    }

    public override void Update()
    {
        base.Update();

        
    }
}
