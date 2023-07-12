using Cinemachine;
using DS.ScriptableObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;

[System.Serializable]
public class MainStreetSubSceneController : SubSceneController
{
    [SerializeField] private DSDialogueSO startingDialogue;

    private bool isFirstEnter = true;

    protected override string subSceneMusic { get => Constants.Audio.Music.MainStreet; }
    public override Constants.Scene7SubScenes Subscene => Constants.Scene7SubScenes.MainStreet;

    public override void Enable(bool teleportPlayer = true)
    {
        base.Enable(teleportPlayer);

        if (isFirstEnter)
        {
            startingDialogue.Interact(this, Constants.Interaction.InteractionType.Virtual);
            isFirstEnter = false;
        }
    }

    public override void Disable()
    {
        base.Disable();
        eventBrokerComponent.Publish(this, new PlayerEvents.GetPlayerPosition(position =>
        {
            subsceneTeleportMarker.position = position;
        }));
    }
}
