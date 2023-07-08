using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class GeneralStoreSubSceneController : SubSceneController
{
    [SerializeField] private ItemInteraction matches;
    [SerializeField] private ItemInteraction twine;

    [SerializeField] private PlayableDirector playableDirector;
    [SerializeField] private PlayableAsset generalStoreStartingCutscene;
    private bool isFirstEnter = true;
    public override void Enable()
    {
        base.Enable();
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
        matches.GetComponent<Collider2D>().enabled = true;
        twine.GetComponent<Collider2D>().enabled = true;
        //matches.InteractionDistance.ConstantValue = 5f;
        //twine.InteractionDistance.ConstantValue = 15f;
    }
}
