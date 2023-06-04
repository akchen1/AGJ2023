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
        // TODO: Deactivate Player movement
        eventBrokerComponent.Publish(this, new InputEvents.SetInputState(false));
    }

    private void NextDialogueHandler(BrokerEvent<DialogueEvents.NextDialogue> inEvent)
    {
        // If there is no current dialogue node (i.e., StartDialogue has not been fired) or
        // dialogue is of type multi, an dialogue option must be selected.
        if (currentDialogue == null || currentDialogue.DialogueType == DS.Enumerations.DSDialogueType.MultipleChoice) return;

        // Check if there is an item event
        if (currentDialogue.Item != null)
        {
            eventBrokerComponent.Publish(this, new InventoryEvents.AddItem(currentDialogue.Item));
        }

        // Check if there is a scene transition event
        if (currentDialogue.HasSceneTransition) // Assuming Bootstrap is 0
        {
            eventBrokerComponent.Publish(this, new SceneEvents.SceneChange(currentDialogue.NextSceneName));
        }

        // Set next dialogue node to current dialogue. If null, that means dialogue is over
        currentDialogue = currentDialogue.Choices[0].NextDialogue;
        // Fire callback
        inEvent.Payload.NextDialogueNode?.Invoke(currentDialogue);

        if (currentDialogue == null)
        {
            eventBrokerComponent.Publish(this, new DialogueEvents.DialogueFinish());
            eventBrokerComponent.Publish(this, new InputEvents.SetInputState(true));
        }
    }

    private void SelectDialogueOptionHandler(BrokerEvent<DialogueEvents.SelectDialogueOption> inEvent)
    {
        if (inEvent.Payload.Option.HasSanityThreshold)
        {
            eventBrokerComponent.Publish(this, new SanityEvents.ChangeSanity((int)inEvent.Payload.Option.SanityType));
        }
        DSDialogueSO nextDialogue = inEvent.Payload.Option.NextDialogue;
        currentDialogue = nextDialogue;
        inEvent.Payload.NextDialogueNode?.Invoke(nextDialogue);
    }
    #endregion
}
