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
    }

    // TODO: Implement when to switch to endCutscene after fixing vase
}
