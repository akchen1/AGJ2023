using DS.ScriptableObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestSceneBirdNestDialogueInteraction : DialogueInteraction
{
    [SerializeField] private DSDialogueSO birdNestChoiceDialogueNode;
    [SerializeField] private GameObject branchBundle;

    protected override void OnEnable()
    {
        base.OnEnable();
        eventBrokerComponent.Subscribe<DialogueEvents.SelectDialogueOption>(SelectDialogueOptionHandler);
    }

    protected override void OnDisable()
    {
        base.OnDisable();
        eventBrokerComponent.Unsubscribe<DialogueEvents.SelectDialogueOption>(SelectDialogueOptionHandler);
    }

    private void SelectDialogueOptionHandler(BrokerEvent<DialogueEvents.SelectDialogueOption> obj)
    {
        if (!isInteracting) return;

        if (birdNestChoiceDialogueNode.Choices[0] == obj.Payload.Option)
        {
            // Break branch choice
            destroyOnDialogueFinish = true;
            eventBrokerComponent.Publish(this, new AudioEvents.PlaySFX(Constants.Audio.SFX.BranchBreak));
        }
        else if (birdNestChoiceDialogueNode.Choices[1] == obj.Payload.Option)
        {
            // Pick different branch
            destroyOnDialogueFinish = false;
            Destroy(GetComponent<BoxCollider2D>());
            eventBrokerComponent.Publish(this, new AudioEvents.PlaySFX(Constants.Audio.SFX.BranchBreak));
        }
    }

    protected override void DialogueFinishHandler(BrokerEvent<DialogueEvents.DialogueFinish> obj)
    {
        if (!isInteracting) return;
        branchBundle.SetActive(true);
        base.DialogueFinishHandler(obj);
    }
}
