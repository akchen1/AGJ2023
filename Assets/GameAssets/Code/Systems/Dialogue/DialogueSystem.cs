using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueSystem 
{
	private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

    public DialogueSystem()
    {
		eventBrokerComponent.Subscribe<DialogueEvents.StartDialogue>(StartDialogueHandler);
    }

    ~DialogueSystem()
    {
        eventBrokerComponent.Unsubscribe<DialogueEvents.StartDialogue>(StartDialogueHandler);
    }

    private void StartDialogueHandler(BrokerEvent<DialogueEvents.StartDialogue> inEvent)
    {
        throw new NotImplementedException();
    }
}
