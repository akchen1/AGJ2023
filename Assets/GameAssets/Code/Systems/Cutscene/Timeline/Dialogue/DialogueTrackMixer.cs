using System;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

public class DialogueTrackMixer : PlayableBehaviour
{
    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

    private Playable playable;
    private Playable rootPlayable;
    private DialogueClipStartBehaviour currentBehaviour;

    public override void OnGraphStart(Playable playable)
    {
        base.OnGraphStart(playable);
        eventBrokerComponent.Subscribe<CutsceneEvents.TryNextDialogue>(TryNextDialogueHandler);
        this.playable = playable;
        rootPlayable = playable.GetGraph().GetRootPlayable(0);
        eventBrokerComponent.Publish(this, new CutsceneEvents.SetRaycastTarget(true));
    }
    
    public override void OnGraphStop(Playable playable)
    {
        base.OnGraphStop(playable);
        eventBrokerComponent.Unsubscribe<CutsceneEvents.TryNextDialogue>(TryNextDialogueHandler);
        eventBrokerComponent.Publish(this, new CutsceneEvents.SetRaycastTarget(false));
    }

    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        TimelineDialogueSystem timelineDialogueSystem = playerData as TimelineDialogueSystem;

        string currentText = "";
        string currentSpeaker = "";
        float currentAlpha = 0f;

        if (!timelineDialogueSystem) return;

        int inputCount = playable.GetInputCount();
        currentBehaviour = null;
        for (int i = 0; i < inputCount; i++)
        {
            float inputWeight = playable.GetInputWeight(i);
            if (inputWeight > 0f)
            {
                ScriptPlayable<DialogueClipStartBehaviour> inputPlayable = (ScriptPlayable<DialogueClipStartBehaviour>)playable.GetInput(i);

                DialogueClipStartBehaviour input = inputPlayable.GetBehaviour();
                currentSpeaker = input.Dialogue.Character?.CharacterName;
                currentText = input.Dialogue.Text;
                currentAlpha = inputWeight;
                currentBehaviour = input;

                if (currentBehaviour.waitForPlayerInput && rootPlayable.GetTime() - (currentBehaviour.EndTime - currentBehaviour.EaseOutTime) > 0.01f)
                {
                    rootPlayable.SetSpeed(0f);
                }
            }
        }
        timelineDialogueSystem.SetDialogue(currentSpeaker, currentText, new Color(1, 1, 1, currentAlpha));
    }


    private void TryNextDialogueHandler(BrokerEvent<CutsceneEvents.TryNextDialogue> inEvent)
    {
        if (currentBehaviour == null) return;

        double speed = rootPlayable.GetSpeed();

        if (currentBehaviour.canSkipDialogue)
            rootPlayable.SetTime((float)currentBehaviour.EndTime);

        if (speed == 0f)
            rootPlayable.SetSpeed(1f);
    }
}
