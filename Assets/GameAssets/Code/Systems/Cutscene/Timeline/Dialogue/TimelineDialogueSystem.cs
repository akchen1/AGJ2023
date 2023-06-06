using DS.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.UI;

public class TimelineDialogueSystem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;
    [SerializeField] private TextMeshProUGUI nameText;
    [SerializeField] private Image textBackground;
    public void SetDialogue(string speaker, string dialogueText, Color dialogueColor)
    {
        nameText.text = speaker;
        nameText.color = dialogueColor;
        text.text = dialogueText;
        text.color = dialogueColor;
        textBackground.color = dialogueColor;

        text.alignment = speaker == "" || speaker == null ? TextAlignmentOptions.Center : TextAlignmentOptions.Left;

    }

}
