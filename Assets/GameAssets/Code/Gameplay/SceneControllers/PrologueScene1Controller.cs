using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PrologueScene1Controller : SceneController
{
    [SerializeField] private PlayableAsset startingCutscene;
    [SerializeField] private PlayableAsset endCutscene;
    [SerializeField] private PlayableDirector playableDirector;
    EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

    private void Start()
    {
        playableDirector.Play(startingCutscene);
		eventBrokerComponent.Publish(this, new AudioEvents.PlayMusic(Constants.Audio.Music.Prologue, true));
        eventBrokerComponent.Publish(this, new PostProcessingEvents.SetVignette(0f));
    }

    private void OnEnable()
    {
        eventBrokerComponent.Subscribe<MinigameEvents.EndMinigame>(EndMinigameHandler);
    }

    private void OnDisable()
    {
        eventBrokerComponent.Unsubscribe<MinigameEvents.EndMinigame>(EndMinigameHandler);
    }

    private void EndMinigameHandler(BrokerEvent<MinigameEvents.EndMinigame> inEvent)
    {
        if (!inEvent.Payload.Completed) return;
        playableDirector.Play(endCutscene);
    }
}
