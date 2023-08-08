using DS.ScriptableObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ForestSceneBirdNestDialogueInteraction : DialogueInteraction
{
    [SerializeField] private DSDialogueSO birdNestBranchChoiceDialogueNode;
    [SerializeField] private DSDialogueSO birdNestCrushChoiceDialogue;

    [SerializeField] private Vector3 leaveEggsPosition;
    [SerializeField] private Vector3 relocateEggsPosition;
    
    [SerializeField] private GameObject branchBundle;

    protected override void OnEnable()
    {
        base.OnEnable();
        birdNestBranchChoiceDialogueNode.Choices[0].OnDialogueChoiceSelected.AddListener(BranchBreakOptionHandler);
        birdNestBranchChoiceDialogueNode.Choices[1].OnDialogueChoiceSelected.AddListener(DifferentBranchOptionHandler);
        birdNestCrushChoiceDialogue.Choices[0].OnDialogueChoiceSelected.AddListener(CrushEggsOptionHandler);
        birdNestCrushChoiceDialogue.Choices[1].OnDialogueChoiceSelected.AddListener(LeaveEggsOptionHandler);
        birdNestCrushChoiceDialogue.Choices[2].OnDialogueChoiceSelected.AddListener(RelocateEggsOptionHandler);
    }
    protected override void OnDisable()
    {
        base.OnDisable();
        birdNestBranchChoiceDialogueNode.Choices[0].OnDialogueChoiceSelected.RemoveListener(BranchBreakOptionHandler);
        birdNestBranchChoiceDialogueNode.Choices[1].OnDialogueChoiceSelected.RemoveListener(DifferentBranchOptionHandler);
        birdNestCrushChoiceDialogue.Choices[0].OnDialogueChoiceSelected.RemoveListener(CrushEggsOptionHandler);
        birdNestCrushChoiceDialogue.Choices[1].OnDialogueChoiceSelected.RemoveListener(LeaveEggsOptionHandler);
        birdNestCrushChoiceDialogue.Choices[2].OnDialogueChoiceSelected.RemoveListener(RelocateEggsOptionHandler);
    }

    private void RelocateEggsOptionHandler()
    {
        destroyOnDialogueFinish = false;
        Destroy(GetComponent<BoxCollider2D>());
        transform.localPosition = relocateEggsPosition;
    }

    private void LeaveEggsOptionHandler()
    {
        destroyOnDialogueFinish = false;
        Destroy(GetComponent<BoxCollider2D>());
        transform.localPosition = leaveEggsPosition;
    }

    private void CrushEggsOptionHandler()
    {
        destroyOnDialogueFinish = true;
    }

    private void DifferentBranchOptionHandler()
    {
        eventBrokerComponent.Publish(this, new AudioEvents.PlaySFX(Constants.Audio.SFX.BranchBreak));
    }
    private void BranchBreakOptionHandler()
    {
        eventBrokerComponent.Publish(this, new AudioEvents.PlaySFX(Constants.Audio.SFX.BranchBreak));
    }

    protected override void DialogueFinishHandler(BrokerEvent<DialogueEvents.DialogueFinish> obj)
    {
        if (!isInteracting) return;
        branchBundle.SetActive(true);
        base.DialogueFinishHandler(obj);
    }
}
