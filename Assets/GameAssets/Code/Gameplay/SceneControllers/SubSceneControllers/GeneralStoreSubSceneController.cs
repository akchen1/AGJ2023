using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class GeneralStoreSubSceneController : SubSceneController
{
    [Header("Minigame completion colliders to enable")]
    [SerializeField] private Collider2D matchesCollider;
    [SerializeField] private Collider2D twineCollider;

    [Header("On store enter cutscene")]
    [SerializeField] private PlayableDirector playableDirector;
    [SerializeField] private PlayableAsset generalStoreStartingCutscene;
    private bool isFirstEnter = true;

    protected override string subSceneMusic { get => Constants.Audio.Music.GeneralStore; }
    public override Constants.Scene7SubScenes Subscene => Constants.Scene7SubScenes.GeneralStore;

    public override void Enable(bool teleportPlayer = true)
    {
        base.Enable(teleportPlayer);
        eventBrokerComponent.Subscribe<MinigameEvents.EndMinigame>(EndMinigameHandler);
        if (isFirstEnter)
        {
            playableDirector.Play(generalStoreStartingCutscene);
            isFirstEnter = false;
        }
    }

    public override void Disable() 
    { 
        base.Disable();
        eventBrokerComponent.Unsubscribe<MinigameEvents.EndMinigame>(EndMinigameHandler);
    }

    private void EndMinigameHandler(BrokerEvent<MinigameEvents.EndMinigame> obj)
    {
        matchesCollider.enabled = true;
        twineCollider.enabled = true;
    }
}
