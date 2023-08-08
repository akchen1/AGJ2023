using DS.ScriptableObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class ForestSubSceneController : SubSceneController
{
    [Header("Obtained all items references")]
    [Tooltip("Dialogue to trigger once all items have been collected and player enters forest")]
    [SerializeField] private DSDialogueSO allItemsDialogue;
    [SerializeField] private GameObject clearingSceneInteraction;

    [Header("Scroll reference")]
    [SerializeField] [Tooltip("Reference to scroll state scriptable object")] 
    private ScrollStateReference scrollStateReference;

    private static bool hasTriggeredAllItemsObtained = false;   // Makes sure all items dialogue only triggers once

    protected override string subSceneMusic { get => Constants.Audio.Music.Forest; }
    public override Constants.Scene7SubScenes Subscene => Constants.Scene7SubScenes.Forest;

    public override void Enable(bool overrideTeleport = false)
    {
        base.Enable(overrideTeleport);

        if (!hasTriggeredAllItemsObtained && scrollStateReference.Value == ScrollState.ItemsObtained)
        {
            allItemsDialogue.Interact(this, Constants.Interaction.InteractionType.Virtual);
            clearingSceneInteraction.GetComponent<DialogueInteraction>().enabled = false;
            clearingSceneInteraction.GetComponent<SceneChangeInteraction>().enabled = true;
            hasTriggeredAllItemsObtained = true;
        }
    }

    public override void Update()
    {
        base.Update();

        
    }
}
