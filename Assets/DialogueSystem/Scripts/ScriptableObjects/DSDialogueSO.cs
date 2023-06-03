using System.Collections;
using System.Collections.Generic;
using UnityEngine;

namespace DS.ScriptableObjects
{
    using Enumerations;
    using Data;

    [System.Serializable]
    public class DSDialogueSO : ScriptableObject
    {
        [field: SerializeField] public string DialogueName { get; set; }
        [field: SerializeField] public Character Character { get; set; }
        [field: SerializeField] public InventoryItem Item { get; set; }
        [field: SerializeField] public bool HasSceneTransition { get; set; }

        [field: SerializeField] public string NextSceneName { get; set; }
        [field: SerializeField] public AudioClip AudioClip { get; set; }
        [field: SerializeField] [field: TextArea()] public string Text { get; set; }
        [field: SerializeField] public List<DSDialogueChoiceData> Choices { get; set; }
        [field: SerializeField] public DSDialogueType DialogueType { get; set; }
        [field: SerializeField] public bool IsStartingDialogue { get; set; }

        public void Initialize(string dialogueName, Character character, InventoryItem item, 
            bool hasSceneTransition, string nextSceneName, AudioClip audioClip, string text, List<DSDialogueChoiceData> choices, 
            DSDialogueType dialogueType, bool isStartingDialogue)
        {
            DialogueName = dialogueName;
            Character = character;
            Item = item;
            HasSceneTransition = hasSceneTransition;
            NextSceneName = nextSceneName;
            AudioClip = audioClip;
            Text = text;
            Choices = choices;
            DialogueType = dialogueType;
            IsStartingDialogue = isStartingDialogue;
        }
    }
}

