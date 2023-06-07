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

    [SerializeField] private PlayableDirector maeveLeaveCutscene;
    [SerializeField] private PlayableDirector leighLeaveCutscene;
    [SerializeField] private PlayableDirector baduLeaveCutscene;

    private DSDialogueSO currentDialogue;

    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();
    void Start()
    {
        eventBrokerComponent.Publish(this, new DialogueEvents.StartDialogue(startingDialogue));
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
            maeveLeaveCutscene.Play();
            maeveLeaveCutscene.stopped += (obj) => { 
                eventBrokerComponent.Publish(this, new DialogueEvents.StartDialogue(onMaeveLeaveDialogue));
                currentDialogue = onMaeveLeaveDialogue;
            };
            // Start onMaeveLeave dialogue
        } else if (currentDialogue == onMaeveLeaveDialogue)
        {
            leighLeaveCutscene.Play();
            leighLeaveCutscene.stopped += (obj) => {
                eventBrokerComponent.Publish(this, new DialogueEvents.StartDialogue(onLeighLeaveDialogue));
                currentDialogue = onLeighLeaveDialogue;
            };
        }  else if (currentDialogue == onLeighLeaveDialogue)
        {
            baduLeaveCutscene.Play();
        }
    }

    private void Test(PlayableDirector obj)
    {
        throw new NotImplementedException();
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
