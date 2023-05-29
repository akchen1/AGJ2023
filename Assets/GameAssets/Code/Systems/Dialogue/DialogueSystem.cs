using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DS.ScriptableObjects;

[System.Serializable]
public class DialogueSystem
{
    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

    private DSDialogueSO currentDialogue;   // Current dialogue node

    public DialogueSystem()
    {
        eventBrokerComponent.Subscribe<DialogueEvents.StartDialogue>(StartDialogueHandler);
        eventBrokerComponent.Subscribe<DialogueEvents.NextDialogue>(NextDialogueHandler);
        eventBrokerComponent.Subscribe<DialogueEvents.SelectDialogueOption>(SelectDialogueOptionHandler);
    }

    ~DialogueSystem()
    {
        eventBrokerComponent.Unsubscribe<DialogueEvents.StartDialogue>(StartDialogueHandler);
        eventBrokerComponent.Unsubscribe<DialogueEvents.NextDialogue>(NextDialogueHandler);
        eventBrokerComponent.Unsubscribe<DialogueEvents.SelectDialogueOption>(SelectDialogueOptionHandler);
    }

    #region Event Handlers
    private void StartDialogueHandler(BrokerEvent<DialogueEvents.StartDialogue> inEvent)
    {
        currentDialogue = inEvent.Payload.StartingDialogue;
    }

    private void NextDialogueHandler(BrokerEvent<DialogueEvents.NextDialogue> inEvent)
    {
        // If there is no current dialogue node (i.e., StartDialogue has not been fired) or
        // dialogue is of type multi, an dialogue option must be selected.
        if (currentDialogue == null || currentDialogue.DialogueType == DS.Enumerations.DSDialogueType.MultipleChoice) return;

        // There is no next dialogue node
        if (currentDialogue.Choices[0].NextDialogue == null)
        {
            // Close dialog, end of dialog
            inEvent.Payload.NextDialogueNode?.Invoke(null);
            currentDialogue = null;
            return;
        }

        // Set next dialogue node to current dialogue
        currentDialogue = currentDialogue.Choices[0].NextDialogue;
        // Fire callback
        inEvent.Payload.NextDialogueNode?.Invoke(currentDialogue);
    }

    private void SelectDialogueOptionHandler(BrokerEvent<DialogueEvents.SelectDialogueOption> inEvent)
    {
        DSDialogueSO nextDialogue = inEvent.Payload.Option.NextDialogue;
        currentDialogue = nextDialogue;
        inEvent.Payload.NextDialogueNode?.Invoke(nextDialogue);
    }
    #endregion
}
