using DS.ScriptableObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class BasementSubsceneController : SubSceneController
{
    public Constants.Sanity.SanityType sanityEventResult { get; private set; }

    [Header("Vial Item Interaction")]
    [Tooltip("Dialogue to play when obtained vial")]
    [SerializeField] private DSDialogueSO vialStartingDialogue;
    private bool isVialDialogue = false;

    protected override string subSceneMusic { get => Constants.Audio.Music.Basement; }
    public override Constants.Scene7SubScenes Subscene => Constants.Scene7SubScenes.Basement;
    public override void Enable(bool teleportPlayer = true)
    {
        base.Enable(teleportPlayer);
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
