using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class CutsceneSystem
{
	private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

	public CutsceneSystem()
    {
        eventBrokerComponent.Subscribe<CutsceneEvents.PlayCutscene>(PlayCutsceneHandler);
    }

    ~CutsceneSystem()
    {
        eventBrokerComponent.Unsubscribe<CutsceneEvents.PlayCutscene>(PlayCutsceneHandler);
    }

    private void PlayCutsceneHandler(BrokerEvent<CutsceneEvents.PlayCutscene> inEvent)
    {
        throw new NotImplementedException();
    }
}
