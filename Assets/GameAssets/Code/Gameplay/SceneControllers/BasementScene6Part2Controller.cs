using DS.ScriptableObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class BasementScene6Part2Controller : MonoBehaviour
{
    // Order of events
    // startingDialoguePart1 -> bookshelfCutscene -> startingDialoguePart2 -> ScrollMinigame -> onscrollCompletedDialogue
    // -> interact with inventory -> scroll manual -> postScrollInspectDialoguePart1 -> mothTransformation -> postScrollInspectDialoguePart2 -> baduTransformation
    [SerializeField] private DSDialogueSO startingDialoguePart1;
    [SerializeField] private DSDialogueSO startingDialoguePart2;
    [SerializeField] private DSDialogueSO onScrollCompletedDialogue;

    [SerializeField] private DSDialogueSO postScrollInspectDialgouePart1;
    [SerializeField] private DSDialogueSO postScrollInspectDialoguePart2;

    [SerializeField] private Animator baduAnimator;

    [SerializeField] private PlayableDirector bookshelfCutscene;

    [SerializeField] private GameObject openedScroll;
    [SerializeField] private GameObject basementStairs;

    private DSDialogueSO currentDialogue;
    private bool isInspectingScroll = false;
    private bool isFirstInspect = true;

    private bool isPlayingMinigame = false;

    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();
    void Start()
    {
        eventBrokerComponent.Publish(this, new DialogueEvents.StartDialogue(startingDialoguePart1));
		eventBrokerComponent.Publish(this, new AudioEvents.PlayMusic(Constants.Audio.Music.Basement, true));
		currentDialogue = startingDialoguePart1;
        baduAnimator.SetTrigger("fly");
    }

    private void OnEnable()
    {
        eventBrokerComponent.Subscribe<DialogueEvents.DialogueFinish>(DialogueFinishHandler);
        eventBrokerComponent.Subscribe<MinigameEvents.EndMinigame>(EndMinigameHandler);
        eventBrokerComponent.Subscribe<MinigameEvents.StartMinigame>(StartMinigameHandler);
    }

    private void OnDisable()
    {
        eventBrokerComponent.Unsubscribe<DialogueEvents.DialogueFinish>(DialogueFinishHandler);
        eventBrokerComponent.Unsubscribe<MinigameEvents.EndMinigame>(EndMinigameHandler);
        eventBrokerComponent.Unsubscribe<MinigameEvents.StartMinigame>(StartMinigameHandler);
    }

    private void DialogueFinishHandler(BrokerEvent<DialogueEvents.DialogueFinish> inEvent)
    {
        if (currentDialogue == startingDialoguePart1)
        {
            bookshelfCutscene.Play();
            bookshelfCutscene.stopped += OnBookshelfCutsceneStopped;
        } else if (currentDialogue == startingDialoguePart2)
        {
            currentDialogue = null;
        }
        else if (currentDialogue == postScrollInspectDialgouePart1)
        {
            baduAnimator.transform.localScale = new Vector3(0.8f, 0.8f, 0.8f);
            baduAnimator.SetBool("isBadu", true);
            eventBrokerComponent.Publish(this, new DialogueEvents.StartDialogue(postScrollInspectDialoguePart2));
            currentDialogue = postScrollInspectDialoguePart2;
        }
        else if (currentDialogue == postScrollInspectDialoguePart2)
        {
            baduAnimator.transform.localScale = Vector3.one;

            baduAnimator.SetBool("isBadu", false);
            baduAnimator.SetTrigger("fly");
            currentDialogue = null;
            basementStairs.SetActive(true);
            eventBrokerComponent.Publish(this, new SceneEvents.SceneChange(Constants.SceneNames.SearchScene7MainStreet));
        }
    }

    private void OnBookshelfCutsceneStopped(PlayableDirector obj)
    {
        eventBrokerComponent.Publish(this, new DialogueEvents.StartDialogue(startingDialoguePart2));
        currentDialogue = startingDialoguePart2;
        bookshelfCutscene.stopped -= OnBookshelfCutsceneStopped;
    }

    private void EndMinigameHandler(BrokerEvent<MinigameEvents.EndMinigame> obj)
    {
        if (isInspectingScroll && isFirstInspect)
        {
            eventBrokerComponent.Publish(this, new DialogueEvents.StartDialogue(postScrollInspectDialgouePart1));
            currentDialogue = postScrollInspectDialgouePart1;
            isFirstInspect = false;
        } else if (isPlayingMinigame)
        {
            eventBrokerComponent.Publish(this, new DialogueEvents.StartDialogue(onScrollCompletedDialogue));
            currentDialogue = onScrollCompletedDialogue;
            isPlayingMinigame = false;
            openedScroll.SetActive(false);
        }
    }
    private void StartMinigameHandler(BrokerEvent<MinigameEvents.StartMinigame> inEvent)
    {
        if (inEvent.Sender.ToString() == "ScrollManual (InventoryInteraction)")
        {
            isInspectingScroll = true;
        }

        if (inEvent.Sender.ToString() == $"{openedScroll.name} (MinigameInteraction)")
        {
            isPlayingMinigame = true;
        }
        
    }
}
