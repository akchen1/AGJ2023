using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;
using UnityEngine.UI;
using DS;
using DS.Data;
using System;

public class DialogueOptionUI : MonoBehaviour
{
    [SerializeField] private TMP_Text text;

    private Button button;

    private void Awake()
    {
        button = GetComponent<Button>();
    }

    public void Initialize(DSDialogueChoiceData dialogueOption, Action<DSDialogueChoiceData> onClickOption)
    {
        text.text = dialogueOption.Text;
        if (button == null) button = GetComponent<Button>();
        button.onClick.AddListener(() => onClickOption?.Invoke(dialogueOption));
    }
}
