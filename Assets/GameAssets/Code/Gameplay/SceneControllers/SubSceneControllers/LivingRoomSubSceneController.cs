using DS.ScriptableObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

[System.Serializable]
public class LivingRoomSubSceneController : SubSceneController
{
    [SerializeField] private InventoryItem pocketKnife;
    [SerializeField] private DSDialogueSO baduSanityDialogue;
    [SerializeField] private DSDialogueSO sanityChoiceDialogueNode;
    [SerializeField] private PlayableDirector director;
    [SerializeField] private PlayableAsset maeveLeaveCutscene;
    [SerializeField] private PlayableAsset vaseBreakCutscene;

    [SerializeField] private Animator baduAnimator;
    private bool pocketKnifeObtained = false;
    private bool isFirstInteract = true;
    private bool sanityDialogueStarted = false;
    public override void Enable()
    {
        base.Enable();
        eventBrokerComponent.Subscribe<InventoryEvents.AddItem>(AddItemHandler);
        eventBrokerComponent.Subscribe<InteractionEvents.InteractEnd>(InteractEndHandler);
        eventBrokerComponent.Subscribe<DialogueEvents.SelectDialogueOption>(SelectDialogueOptionHandler);
        eventBrokerComponent.Subscribe<DialogueEvents.DialogueFinish>(DialogueFinishHandler);
        
        baduAnimator.SetTrigger(pocketKnifeObtained ? "fly" : "idle");

        
    }

    public override void Disable()
    {
        base.Disable();
        eventBrokerComponent.Unsubscribe<InventoryEvents.AddItem>(AddItemHandler);
        eventBrokerComponent.Unsubscribe<InteractionEvents.InteractEnd>(InteractEndHandler);
        eventBrokerComponent.Unsubscribe<DialogueEvents.SelectDialogueOption>(SelectDialogueOptionHandler);
        eventBrokerComponent.Unsubscribe<DialogueEvents.DialogueFinish>(DialogueFinishHandler);
    }

    private void DialogueFinishHandler(BrokerEvent<DialogueEvents.DialogueFinish> obj)
    {
        if (!sanityDialogueStarted) return;
        baduAnimator.SetBool("isBadu", false);
        baduAnimator.SetTrigger("fly");
        sanityDialogueStarted = false;
    }

    private void SelectDialogueOptionHandler(BrokerEvent<DialogueEvents.SelectDialogueOption> obj)
    {
        if (!sanityDialogueStarted) return;
        if (obj.Payload.Option == sanityChoiceDialogueNode.Choices[0])
        {
            director.Play(vaseBreakCutscene);
        }
    }

    private void AddItemHandler(BrokerEvent<InventoryEvents.AddItem> obj)
    {
        foreach (InventoryItem item in obj.Payload.Items)
        {
            if (item == pocketKnife)
            {
                pocketKnifeObtained = true;
            }
        }
    }

    private void InteractEndHandler(BrokerEvent<InteractionEvents.InteractEnd> obj)
    {
        if (pocketKnifeObtained && isFirstInteract)
        {
            director.Play(maeveLeaveCutscene);
            director.stopped += OnMaeveLeaveCutsceneFinish;
            isFirstInteract = false;
        }
    }

    private void OnMaeveLeaveCutsceneFinish(PlayableDirector obj)
    {
        baduSanityDialogue.Interact(this, Constants.Interaction.InteractionType.Virtual);
        director.stopped -= OnMaeveLeaveCutsceneFinish;
        sanityDialogueStarted = true;
    }
}
