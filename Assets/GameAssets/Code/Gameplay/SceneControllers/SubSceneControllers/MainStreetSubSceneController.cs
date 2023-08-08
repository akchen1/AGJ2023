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

    public override void Enable(bool overrideTeleport = false)
    {

        if (isFirstEnter)
        {
            teleportMaeve = false;
            base.Enable(overrideTeleport);
            teleportMaeve = true;
            eventBrokerComponent.Publish(this, new Scene7Events.EnableMaeveMovement(false));
            eventBrokerComponent.Subscribe<DialogueEvents.DialogueFinish>(DialogueFinishHandler);
            startingDialogue.Interact(this, Constants.Interaction.InteractionType.Virtual);
            isFirstEnter = false;
        } else
        {
            base.Enable(overrideTeleport);
        }
    }

    private void DialogueFinishHandler(BrokerEvent<DialogueEvents.DialogueFinish> inEvent)
    {
        eventBrokerComponent.Publish(this, new Scene7Events.EnableMaeveMovement(true));
        eventBrokerComponent.Unsubscribe<DialogueEvents.DialogueFinish>(DialogueFinishHandler);
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
