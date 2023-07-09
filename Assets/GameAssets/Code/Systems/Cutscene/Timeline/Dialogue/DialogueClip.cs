using DS.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class DialogueClip : PlayableAsset
{
    public DSDialogueSO Dialogue;
    public bool canSkipDialogue;
    public bool waitForPlayerInput;

    [HideInInspector] public double StartTime;
    [HideInInspector] public double EndTime;
    [HideInInspector] public double EaseInTime;
    [HideInInspector] public double EaseOutTime;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        ScriptPlayable<DialogueClipStartBehaviour> playable = ScriptPlayable<DialogueClipStartBehaviour>.Create(graph);

        DialogueClipStartBehaviour subtitleBehaviour = playable.GetBehaviour();
        subtitleBehaviour.Dialogue = Dialogue;
        subtitleBehaviour.canSkipDialogue = canSkipDialogue;
        subtitleBehaviour.waitForPlayerInput = waitForPlayerInput;
        subtitleBehaviour.EndTime = EndTime;
        subtitleBehaviour.StartTime = StartTime;
        subtitleBehaviour.EaseInTime = EaseInTime;
        subtitleBehaviour.EaseOutTime = EaseOutTime;
        return playable;
    }
}
