using DS.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimelineDialogueSystem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private TextMeshProUGUI nameText;
    public void SetDialogue(string speaker, string dialogueText, Color dialogueColor)
    {
        nameText.text = speaker;
        nameText.color = dialogueColor;
        text.text = dialogueText;
        text.color = dialogueColor;
    }

}
