using DS.ScriptableObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ForestSubSceneController : SubSceneController
{
    [SerializeField] private DSDialogueSO allComponentsDialogue;
    [SerializeField] private GameObject clearingSceneInteraction;
    [SerializeField] private ScrollStateReference scrollStateReference;

    private static bool hasTriggeredAllItemsObtained = false;

    public override void Enable()
    {
        base.Enable();

        if (!hasTriggeredAllItemsObtained && scrollStateReference.Value == ScrollState.ItemsObtained)
        {
            allComponentsDialogue.Interact(this, Constants.Interaction.InteractionType.Virtual);
            clearingSceneInteraction.SetActive(true);
            hasTriggeredAllItemsObtained = true;
        }
    }

    public override void Update()
    {
        base.Update();

        
    }
}
