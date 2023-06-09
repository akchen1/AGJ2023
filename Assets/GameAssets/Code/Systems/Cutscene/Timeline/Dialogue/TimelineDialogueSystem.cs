using DS.ScriptableObjects;
using System;
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
    [SerializeField] private Image cutsceneDialogueBackground;

    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

    private void OnEnable()
    {
        eventBrokerComponent.Subscribe<CutsceneEvents.SetRaycastTarget>(SetRaycastTargetHandler);
    }

    private void OnDisable()
    {
        eventBrokerComponent.Unsubscribe<CutsceneEvents.SetRaycastTarget>(SetRaycastTargetHandler);
    }

    private void SetRaycastTargetHandler(BrokerEvent<CutsceneEvents.SetRaycastTarget> obj)
    {
        cutsceneDialogueBackground.raycastTarget = obj.Payload.Active;
    }

    public void SetDialogue(string speaker, string dialogueText, Color dialogueColor)
    {
        nameText.text = speaker;
        nameText.color = dialogueColor;
        text.text = dialogueText;
        text.color = dialogueColor;
        textBackground.color = dialogueColor;

        text.alignment = speaker == "" || speaker == null ? TextAlignmentOptions.Center : TextAlignmentOptions.Left;

    }

    public void TryNextDialogue()
    {
        eventBrokerComponent.Publish(this, new CutsceneEvents.TryNextDialogue());
    }

    public void SetBackgroundRaycastTarget(bool active)
    {
        cutsceneDialogueBackground.raycastTarget = active;
    }
}
