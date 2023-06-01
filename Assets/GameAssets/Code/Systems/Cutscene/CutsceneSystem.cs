using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class CutsceneSystem
{
    private PlayableDirector director;

    private PlayableAsset currentCutscene;

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
        currentCutscene = inEvent.Payload.Cutscene;
        director.Play(currentCutscene);
    }
}
