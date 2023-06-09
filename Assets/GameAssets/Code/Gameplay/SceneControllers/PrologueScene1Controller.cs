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

	private bool recordPlayerPlaying = false;

    private void Start()
    {
        playableDirector.Play(startingCutscene);
		eventBrokerComponent.Publish(this, new AudioEvents.PlayMusic(Constants.Audio.Music.Prologue));
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
        playableDirector.Play(endCutscene);
    }

	public void ToggleRecordPlayer()
	{
		if (!recordPlayerPlaying)
		{
			eventBrokerComponent.Publish(this, new AudioEvents.PlayTemporaryMusic(Constants.Audio.Music.RecordPlayer));
		}
		else
		{
			eventBrokerComponent.Publish(this, new AudioEvents.StopTemporaryMusic());
		}

		recordPlayerPlaying = !recordPlayerPlaying;
	}
}
