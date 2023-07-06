using DS.Data;
using DS.ScriptableObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class Scene9Controller : SceneController
{
    [SerializeField] private List<InventoryItem> startingItems;
    [SerializeField] private PlayableAsset startingCutscene;
    [SerializeField] private PlayableAsset closedScrollCutscene;
    [SerializeField] private PlayableDirector playableDirector;

    [SerializeField] private DSDialogueSO completedRitualSetupDialogue;
    [SerializeField] private DSDialogueSO closedScrollDialogue;
    [SerializeField] private DSDialogueSO endingSceneDialogue;
    [SerializeField] private ScrollStateReference scrollStateReference;

    [SerializeField] private DSDialogueSO saneEndingDialogueNode;
    [SerializeField] private DSDialogueSO insaneEndingDialogueNode;
    [SerializeField] private PlayableAsset saneEndingCutscene;
    [SerializeField] private PlayableAsset insaneEndingCutscene;
    private DSDialogueChoiceData saneChoice { get { return saneEndingDialogueNode.Choices[1]; } }
    private DSDialogueChoiceData insaneChoice { get { return insaneEndingDialogueNode.Choices[0]; } }

    EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

	private bool recordPlayerPlaying = false;
    private void Start()
    {
        //endingSceneDialogue.Interact(this, Constants.Interaction.InteractionType.Virtual);

        playableDirector.Play(startingCutscene);
        eventBrokerComponent.Publish(this, new AudioEvents.PlayMusic(Constants.Audio.Music.Clearing));
        eventBrokerComponent.Publish(this, new InventoryEvents.AddItem(startingItems.ToArray()));
    }

    private void OnEnable()
    {
        eventBrokerComponent.Subscribe<MinigameEvents.EndMinigame>(EndMinigameHandler);
        eventBrokerComponent.Subscribe<DialogueEvents.SelectDialogueOption>(SelectDialogueOptionHandler);
    }

    private void OnDisable()
    {
        eventBrokerComponent.Unsubscribe<MinigameEvents.EndMinigame>(EndMinigameHandler);
        eventBrokerComponent.Unsubscribe<DialogueEvents.SelectDialogueOption>(SelectDialogueOptionHandler);
    }

    private void SelectDialogueOptionHandler(BrokerEvent<DialogueEvents.SelectDialogueOption> obj)
    {
        if (obj.Payload.Option == saneChoice)
        {
            playableDirector.Play(saneEndingCutscene);
        } else if (obj.Payload.Option == insaneChoice)
        {
            playableDirector.Play(insaneEndingCutscene);
        }
    }

    private void EndMinigameHandler(BrokerEvent<MinigameEvents.EndMinigame> inEvent)
    {
        Type miniGame = inEvent.Payload.Minigame.GetType();
        if (miniGame == typeof(RitualMinigame))
        {
            completedRitualSetupDialogue.Interact(this, Constants.Interaction.InteractionType.Virtual);
            scrollStateReference.Variable.SetValue(ScrollState.RitualComplete);
        } else if (miniGame == typeof(ScrollManual) && scrollStateReference.Value == ScrollState.RitualComplete)
        {
            playableDirector.Play(closedScrollCutscene);
            playableDirector.stopped += CloseScrollCutsceneStoppedHandler;
        }
    }

    private void CloseScrollCutsceneStoppedHandler(PlayableDirector obj)
    {
        obj.stopped -= CloseScrollCutsceneStoppedHandler;
        endingSceneDialogue.Interact(this, Constants.Interaction.InteractionType.Virtual);
    }
}
