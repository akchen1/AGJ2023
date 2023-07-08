using DS.ScriptableObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class PlaygroundScene5Controller : MonoBehaviour
{
    [SerializeField] private DSDialogueSO startingDialogue;
    [SerializeField] private DSDialogueSO onMaeveLeaveDialogue;
    [SerializeField] private DSDialogueSO onLeighLeaveDialogue;

    [SerializeField] private PlayableDirector director;
    [SerializeField] private PlayableAsset maeveLeaveCutscene;
    [SerializeField] private PlayableAsset leighLeaveCutscene;
    [SerializeField] private PlayableAsset baduLeaveCutscene;

    private DSDialogueSO currentDialogue;

    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();
    void Start()
    {
        startingDialogue.Interact(this, Constants.Interaction.InteractionType.Virtual);
		eventBrokerComponent.Publish(this, new AudioEvents.PlayMusic(Constants.Audio.Music.Playtime, true));
		currentDialogue = startingDialogue;
    }

    private void OnEnable()
    {
        eventBrokerComponent.Subscribe<DialogueEvents.DialogueFinish>(DialogueFinishHandler);
    }

    private void OnDisable()
    {
        eventBrokerComponent.Unsubscribe<DialogueEvents.DialogueFinish>(DialogueFinishHandler);
    }

    private void DialogueFinishHandler(BrokerEvent<DialogueEvents.DialogueFinish> inEvent)
    {
        if (currentDialogue == startingDialogue)
        {
            // Trigger Maeve leave
            director.Play(maeveLeaveCutscene);
            director.stopped += OnMaeveLeaveCutsceneStopped;
            // Start onMaeveLeave dialogue
        } else if (currentDialogue == onMaeveLeaveDialogue)
        {
            director.Play(leighLeaveCutscene);
            director.stopped += OnLeighLeaveCutsceneStopped;
        }  else if (currentDialogue == onLeighLeaveDialogue)
        {
            director.Play(baduLeaveCutscene);
        }
    }

    private void OnLeighLeaveCutsceneStopped(PlayableDirector obj)
    {
        onLeighLeaveDialogue.Interact(this, Constants.Interaction.InteractionType.Virtual);
        currentDialogue = onLeighLeaveDialogue;
        director.stopped -= OnLeighLeaveCutsceneStopped;
    }

    private void OnMaeveLeaveCutsceneStopped(PlayableDirector obj)
    {
        onMaeveLeaveDialogue.Interact(this, Constants.Interaction.InteractionType.Virtual);
        currentDialogue = onMaeveLeaveDialogue;
        director.stopped -= OnMaeveLeaveCutsceneStopped;
    }
}
