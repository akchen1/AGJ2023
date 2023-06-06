using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

public class DialogueTrackMixer : PlayableBehaviour
{
    public override void ProcessFrame(Playable playable, FrameData info, object playerData)
    {
        TimelineDialogueSystem timelineDialogueSystem = playerData as TimelineDialogueSystem;
        string currentText = "";
        string currentSpeaker = "";
        float currentAlpha = 0f;

        if (!timelineDialogueSystem) return;

        int inputCount = playable.GetInputCount();
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
            }
        }
        timelineDialogueSystem.SetDialogue(currentSpeaker, currentText, new Color(1, 1, 1, currentAlpha));
    }
}
