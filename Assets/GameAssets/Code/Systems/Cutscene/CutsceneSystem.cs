using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CutsceneSystem
{
    public CutsceneSystem()
    {
        Bootstrap.EventBrokerComponent.Subscribe<Event.PlayCutscene>(PlayCutsceneHandler);
    }

    ~CutsceneSystem()
    {
        Bootstrap.EventBrokerComponent.Unsubscribe<Event.PlayCutscene>(PlayCutsceneHandler);
    }

    private void PlayCutsceneHandler(BrokerEvent<Event.PlayCutscene> inEvent)
    {
        throw new NotImplementedException();
    }
}
