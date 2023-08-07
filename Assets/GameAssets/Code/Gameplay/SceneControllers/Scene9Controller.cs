using DS.Data;
using DS.ScriptableObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using UnityEngine.Playables;

public class Scene9Controller : SceneController
{
    [SerializeField] private GameObject ritualTree;
    [SerializeField] private InventoryItem litCandle;

#if UNITY_EDITOR
    [SerializeField] private List<InventoryItem> startingItems; // testing purposes only
#endif
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

    private void Start()
    {
        //endingSceneDialogue.Interact(this, Constants.Interaction.InteractionType.Virtual);

        playableDirector.Play(startingCutscene);
        eventBrokerComponent.Publish(this, new AudioEvents.PlayMusic(Constants.Audio.Music.Clearing));

#if UNITY_EDITOR
        eventBrokerComponent.Publish(this, new InventoryEvents.AddItem(startingItems.ToArray()));
#endif
    }

    private void OnEnable()
    {
        eventBrokerComponent.Subscribe<MinigameEvents.EndMinigame>(EndMinigameHandler);
        eventBrokerComponent.Subscribe<InventoryEvents.AddItem>(AddItemHandler);

        saneChoice.OnDialogueChoiceSelected.AddListener(OnSaneChoiceSelected);
        insaneChoice.OnDialogueChoiceSelected.AddListener(OnInsaneChoiceSelected);

        ritualTree.GetComponent<MinigameInteraction>().onMinigameFinish.AddListener(OnRitualTreeFinish);
    }

    private void OnDisable()
    {
        eventBrokerComponent.Unsubscribe<MinigameEvents.EndMinigame>(EndMinigameHandler);
        eventBrokerComponent.Unsubscribe<InventoryEvents.AddItem>(AddItemHandler);
        saneChoice.OnDialogueChoiceSelected.RemoveListener(OnSaneChoiceSelected);
        insaneChoice.OnDialogueChoiceSelected.RemoveListener(OnInsaneChoiceSelected);

        ritualTree.GetComponent<MinigameInteraction>().onMinigameFinish.RemoveListener(OnRitualTreeFinish);

    }

    private void OnRitualTreeFinish()
    {
        completedRitualSetupDialogue.Interact(this, Constants.Interaction.InteractionType.Virtual);
        scrollStateReference.Variable.SetValue(ScrollState.RitualComplete);
    }

    private void OnInsaneChoiceSelected()
    {
        playableDirector.Play(insaneEndingCutscene);
    }

    private void OnSaneChoiceSelected()
    {
        playableDirector.Play(saneEndingCutscene);
    }

    private void AddItemHandler(BrokerEvent<InventoryEvents.AddItem> obj)
    {
        if (obj.Payload.Items.Contains(litCandle))
        {
            ritualTree.GetComponent<DialogueInteraction>().enabled = false;
            ritualTree.GetComponent<MinigameInteraction>().enabled = true;
        }
    }

    private void EndMinigameHandler(BrokerEvent<MinigameEvents.EndMinigame> inEvent)
    {
        Type miniGame = inEvent.Payload.Minigame.GetType();
        if (miniGame == typeof(ScrollManual) && scrollStateReference.Value == ScrollState.RitualComplete)
        {
            playableDirector.Play(closedScrollCutscene);
            playableDirector.stopped += CloseScrollCutsceneStoppedHandler;
        }
    }

    private void CloseScrollCutsceneStoppedHandler(PlayableDirector obj)
    {
        obj.stopped -= CloseScrollCutsceneStoppedHandler;
        endingSceneDialogue.Interact(this, Constants.Interaction.InteractionType.Virtual);
        eventBrokerComponent.Publish(this, new AudioEvents.PlayMusic(Constants.Audio.Music.EndingTree, true));
    }
}
