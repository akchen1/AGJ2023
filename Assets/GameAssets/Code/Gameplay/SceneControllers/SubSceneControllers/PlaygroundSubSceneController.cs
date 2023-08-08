using DS.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class PlaygroundSubSceneController : SubSceneController
{
    [Header("Clay minigame")]
    [SerializeField] private InventoryItem clayItem;
    [Tooltip("Dialogue to trigger after obtaining clay")]
    [SerializeField] private DSDialogueSO clayDialogue;
    [Tooltip("Decision node to either break sandcastle or not")]
    [SerializeField] private DSDialogueSO clayDecisionNode;
    [SerializeField] private GameObject sandCastle;

    private bool obtainedClay = false;
    private bool clayDialogueStarted = false;

    protected override string subSceneMusic => Constants.Audio.Music.Playtime;
    public override Constants.Scene7SubScenes Subscene => Constants.Scene7SubScenes.Playground;

    public override void Enable(bool overrideTeleport = false)
    {
        base.Enable(overrideTeleport);
        eventBrokerComponent.Subscribe<InventoryEvents.AddItem>(AddItemHandler);
        eventBrokerComponent.Subscribe<InteractionEvents.InteractEnd>(InteractEndHandler);
        eventBrokerComponent.Subscribe<DialogueEvents.SelectDialogueOption>(SelectDialogueOptionHandler);
    }

    public override void Disable()
    {
        base.Disable();

        eventBrokerComponent.Unsubscribe<InventoryEvents.AddItem>(AddItemHandler);
        eventBrokerComponent.Unsubscribe<InteractionEvents.InteractEnd>(InteractEndHandler);
        eventBrokerComponent.Unsubscribe<DialogueEvents.SelectDialogueOption>(SelectDialogueOptionHandler);
    }

    private void SelectDialogueOptionHandler(BrokerEvent<DialogueEvents.SelectDialogueOption> obj)
    {
        if (!clayDialogueStarted) return;
        if (obj.Payload.Option == clayDecisionNode.Choices[0])
        {
            sandCastle.SetActive(false);
        }
    }

    private void AddItemHandler(BrokerEvent<InventoryEvents.AddItem> obj)
    {
        foreach (InventoryItem item in obj.Payload.Items)
        {
            if (item == clayItem)
            {
                obtainedClay = true;
                return;
            }
        }
    }

    private void InteractEndHandler(BrokerEvent<InteractionEvents.InteractEnd> obj)
    {
        if (clayDialogueStarted)
        {
            clayDialogueStarted = false;
        }
        if (obtainedClay)
        {
            clayDialogueStarted = clayDialogue.Interact(this, Constants.Interaction.InteractionType.Virtual);
            obtainedClay = false;
            return;
        }
    }
}
