using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using DS.ScriptableObjects;
using DS.Data;

public class DialogueUI : MonoBehaviour
{
    [SerializeField] private GameObject dialoguePanel;  // Highest level panel that contains Dialogue System UI
    [SerializeField] private TMP_Text dialogueText; // Dialogue text to change
    [SerializeField] private TMP_Text dialogueName;
    [SerializeField] private Transform dialogueOptionParent;    // Parent to instantiate dialogue options to
    [SerializeField] private DialogueOptionUI dialogueOptionPrefab; // Dialogue option prefab
    [SerializeField] private GameObject nextDialogueIcon;

    private List<DialogueOptionUI> dialogueOptions = new List<DialogueOptionUI>(); // Stores instantiated dialogue options

    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();
    private void OnEnable()
    {
        eventBrokerComponent.Subscribe<DialogueEvents.StartDialogue>(StartDialogueHandler);
    }

    private void OnDisable()
    {
        eventBrokerComponent.Unsubscribe<DialogueEvents.StartDialogue>(StartDialogueHandler);
    }

    #region Events
    private void StartDialogueHandler(BrokerEvent<DialogueEvents.StartDialogue> inEvent)
    {
        DSDialogueSO dialogue = inEvent.Payload.StartingDialogue;
        SetDialogue(dialogue);

        dialoguePanel.SetActive(true);
    }
    #endregion

    #region UI
    public void NextDialogue()
    {
        // Event fired when player clicks next
        eventBrokerComponent.Publish(this, new DialogueEvents.NextDialogue((NextDialogue) =>
        {
            SetDialogue(NextDialogue);
        }));
    }

    private void OnDialogueOptionSelected(DSDialogueChoiceData dialogueChoice)
    {
        // Event fired when player clicks on a dialogue option 
        eventBrokerComponent.Publish(this, new DialogueEvents.SelectDialogueOption(dialogueChoice, (dialogue) =>
        {
            SetDialogue(dialogue);
        }));
    }
    #endregion

    #region Utility Methods
    private void SetDialogue(DSDialogueSO dialogue)
    {
        if (dialogue == null)
        {
            // If there is no dialogue, close the dialogue panel
            EndDialogue();
            return;
        }

        SetDialogueText(dialogue);
        ClearDialogueOptions();

        // Only instantiate dialogue options if of type multichoice
        if (dialogue.DialogueType == DS.Enumerations.DSDialogueType.MultipleChoice)
        {
            CreateDialogueOptions(dialogue.Choices);
            
        }
        nextDialogueIcon.SetActive(dialogue.DialogueType == DS.Enumerations.DSDialogueType.SingleChoice);
    }

    private void SetDialogueText(DSDialogueSO dialogue)
    {
        dialogueText.text = dialogue.Text;
        dialogueName.text = dialogue.Character.CharacterName;
    }

    private void CreateDialogueOptions(List<DSDialogueChoiceData> choices)
    {
        foreach (DSDialogueChoiceData choice in choices)
        {
            DialogueOptionUI dialogueOptionUI = Instantiate(dialogueOptionPrefab, dialogueOptionParent);
            dialogueOptionUI.Initialize(choice, OnDialogueOptionSelected);
            dialogueOptions.Add(dialogueOptionUI);
        }
    }

    private void ClearDialogueOptions()
    {
        foreach (DialogueOptionUI option in dialogueOptions)
        {
            Destroy(option.gameObject);
        }

        dialogueOptions.Clear();
    }

    private void EndDialogue()
    {
        ClearDialogueOptions();
        dialoguePanel.SetActive(false);
    }
    #endregion
}
