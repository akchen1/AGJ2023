using DS.ScriptableObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Scene9Controller : SceneController
{
    [SerializeField] private PlayableAsset startingCutscene;
    [SerializeField] private PlayableAsset endCutscene;
    [SerializeField] private PlayableDirector playableDirector;

    [SerializeField] private DSDialogueSO completedRitualSetupDialogue;

    EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

	private bool recordPlayerPlaying = false;

    private void Start()
    {
        playableDirector.Play(startingCutscene);
		eventBrokerComponent.Publish(this, new AudioEvents.PlayMusic(Constants.Audio.Music.Clearing));
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
        eventBrokerComponent.Publish(this, new DialogueEvents.StartDialogue(completedRitualSetupDialogue));
    }
}
