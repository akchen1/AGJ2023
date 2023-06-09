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

    private bool isSanityDialogue = false;
    public override void Enable()
    {
        base.Enable();
        eventBrokerComponent.Subscribe<DialogueEvents.StartDialogue>(StartDialogueHandler);
        eventBrokerComponent.Subscribe<DialogueEvents.DialogueFinish>(DialogueFinishHandler);
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
}
