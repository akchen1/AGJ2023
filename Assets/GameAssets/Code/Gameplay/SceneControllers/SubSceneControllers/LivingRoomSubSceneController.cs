using DS.ScriptableObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class LivingRoomSubSceneController : SubSceneController
{
    [Header("Drawer minigame")]
    [SerializeField] private NoConditionMinigame drawerMinigame;
    [Tooltip("Inventory item that triggers maeve leave cutscene")]
    [SerializeField] private InventoryItem pocketKnife;
    [Tooltip("Dialogue to play once maeve exits the room")]
    [SerializeField] private DSDialogueSO baduSanityDialogue;

    [Tooltip("Dialogue node of to either break or not break the vase")]
    [SerializeField] private DSDialogueSO sanityChoiceDialogueNode;

    [Header("Record Player")]
    [SerializeField] private RecordPlayerInteraction recordPlayer;

    [Header("Cutscenes")]
    [SerializeField] private PlayableDirector director;
    
    [Tooltip("Triggered when pocket knife is obtained")]
    [SerializeField] private PlayableAsset maeveLeaveCutscene;

    [Tooltip("Trigged when break sanity choice is chosen")]
    [SerializeField] private PlayableAsset vaseBreakCutscene;

    [Header("Animators")]
    [SerializeField] private Animator baduAnimator;

    private bool pocketKnifeObtained = false;
    private bool isFirstInteract = true;
    private bool sanityDialogueStarted = false;
    protected override string subSceneMusic { get => Constants.Audio.Music.LivingRoom; }
    public override Constants.Scene7SubScenes Subscene => Constants.Scene7SubScenes.LivingRoom;

    public override void Enable(bool teleportPlayer = true)
    {
        base.Enable(teleportPlayer);
        eventBrokerComponent.Subscribe<DialogueEvents.DialogueFinish>(DialogueFinishHandler);

        drawerMinigame.OnMinigameFinish.AddListener(DrawerMinigameFinishHandler);
        sanityChoiceDialogueNode.Choices[0].OnDialogueChoiceSelected.AddListener(BreakVaseHandler);


        baduAnimator.SetTrigger(pocketKnifeObtained ? "fly" : "idle");
    }

    public override void Disable()
    {
        base.Disable();
        recordPlayer.TurnOff();
        eventBrokerComponent.Unsubscribe<DialogueEvents.DialogueFinish>(DialogueFinishHandler);
        drawerMinigame.OnMinigameFinish.RemoveListener(DrawerMinigameFinishHandler);
        sanityChoiceDialogueNode.Choices[0].OnDialogueChoiceSelected.RemoveListener(BreakVaseHandler);
    }

    private void BreakVaseHandler()
    {
        director.Play(vaseBreakCutscene);
    }

    private void DrawerMinigameFinishHandler()
    {
        if (!isFirstInteract) return;
        if (!pocketKnife.CheckInInventory(this)) return;
        if (director.Interact())
        {
            director.Play(maeveLeaveCutscene);
            director.stopped += OnMaeveLeaveCutsceneFinish;
            isFirstInteract = false;
        }
    }


    private void DialogueFinishHandler(BrokerEvent<DialogueEvents.DialogueFinish> obj)
    {
        if (!sanityDialogueStarted) return;
        baduAnimator.SetBool("isBadu", false);
        sanityDialogueStarted = false;
    }

    private void OnMaeveLeaveCutsceneFinish(PlayableDirector obj)
    {
        eventBrokerComponent.Publish(this, new DialogueEvents.StartDialogue(baduSanityDialogue));
        director.stopped -= OnMaeveLeaveCutsceneFinish;
        sanityDialogueStarted = true;
    }
}
