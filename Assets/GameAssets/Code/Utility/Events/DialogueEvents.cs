using DS.Data;
using DS.ScriptableObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueEvents
{
    // Use this event if starting a dialogue for the first time
    public class StartDialogue
    {
        public readonly DSDialogueSO StartingDialogue;

        public StartDialogue(DSDialogueSO startingDialogue)
        {
            StartingDialogue = startingDialogue;
        }
    }

    // Use this event to go to the next dialogue in a single choice dialogue node
    public class NextDialogue
    {
        public readonly Action<DSDialogueSO> NextDialogueNode;

        public NextDialogue(Action<DSDialogueSO> nextDialogueNode)
        {
            NextDialogueNode = nextDialogueNode;
        }
    }

    // Use this event to go to the next dialogue node asociated with the dialogue option
    public class SelectDialogueOption
    {
        public readonly DSDialogueChoiceData Option;    // Dialogue Option selected
        public readonly Action<DSDialogueSO> NextDialogueNode;  // Next Dialogue Node

        public SelectDialogueOption(DSDialogueChoiceData option, Action<DSDialogueSO> nextDialogueNode)
        {
            Option = option;
            NextDialogueNode = nextDialogueNode;
        }
    }
}
