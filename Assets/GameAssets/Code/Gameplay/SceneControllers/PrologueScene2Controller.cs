using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PrologueScene2Controller : SceneController
{
    [SerializeField] private PlayableAsset startingCutscene;
    [SerializeField] private PlayableAsset endCutscene;
    [SerializeField] private PlayableDirector playableDirector;
    EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

    private void Start()
    {
        playableDirector.Play(startingCutscene);
#if UNITY_EDITOR
        eventBrokerComponent.Publish(this, new AudioEvents.PlayMusic(Constants.Audio.Music.Prologue, true));
#endif
        eventBrokerComponent.Publish(this, new PostProcessingEvents.SetVignette(0.05f));
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
}
