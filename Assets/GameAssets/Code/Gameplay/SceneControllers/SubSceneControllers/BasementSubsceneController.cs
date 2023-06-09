using DS.ScriptableObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BasementSubsceneController : SubSceneController
{
    public Constants.Sanity.SanityType sanityEventResult { get; private set; }
    [SerializeField] private DSDialogueSO vialStartingDialogue;
    private bool isVialDialogue = false;

    public override void Enable()
    {
        base.Enable();
        eventBrokerComponent.Subscribe<DialogueEvents.SelectDialogueOption>(SelectDialogueOptionHandler);
        eventBrokerComponent.Subscribe<DialogueEvents.StartDialogue>(StartDialogueHandler);
    }
    public override void Disable()
    {
        base.Disable();
        eventBrokerComponent.Unsubscribe<DialogueEvents.SelectDialogueOption>(SelectDialogueOptionHandler);
        eventBrokerComponent.Unsubscribe<DialogueEvents.StartDialogue>(StartDialogueHandler);
    }

    private void StartDialogueHandler(BrokerEvent<DialogueEvents.StartDialogue> inEvent)
    {
        if (inEvent.Payload.StartingDialogue == vialStartingDialogue)
        {
            isVialDialogue = true;
        }
    }

    private void SelectDialogueOptionHandler(BrokerEvent<DialogueEvents.SelectDialogueOption> inEvent)
    {
        if (!isVialDialogue) return;
        sanityEventResult = inEvent.Payload.Option.SanityType;
    }
}
