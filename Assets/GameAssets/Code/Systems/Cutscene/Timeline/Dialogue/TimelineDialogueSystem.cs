using DS.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;

public class TimelineDialogueSystem : MonoBehaviour
{
    [SerializeField] private TextMeshProUGUI text;

    public void SetDialogue(string dialogueText, Color dialogueColor)
    {
        text.text = dialogueText;
        text.color = dialogueColor;
    }

}
