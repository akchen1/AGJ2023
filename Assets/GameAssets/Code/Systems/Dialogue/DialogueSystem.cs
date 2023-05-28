using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class DialogueSystem {
    public DialogueSystem()
    {
        Bootstrap.EventBrokerComponent.Subscribe<Event.StartDialogue>(StartDialogueHandler);
    }

    ~DialogueSystem()
    {
        Bootstrap.EventBrokerComponent.Unsubscribe<Event.StartDialogue>(StartDialogueHandler);
    }

    private void StartDialogueHandler(BrokerEvent<Event.StartDialogue> obj)
    {
        throw new NotImplementedException();
    }
}
