using DS.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class DialogueClip : PlayableAsset
{
    public DSDialogueSO Dialogue;
    public bool canSkipDialogue;

    [HideInInspector] public double StartTime;
    [HideInInspector] public double EndTime;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        ScriptPlayable<DialogueClipStartBehaviour> playable = ScriptPlayable<DialogueClipStartBehaviour>.Create(graph);

        DialogueClipStartBehaviour subtitleBehaviour = playable.GetBehaviour();
        subtitleBehaviour.Dialogue = Dialogue;
        subtitleBehaviour.canSkipDialogue = canSkipDialogue;
        subtitleBehaviour.EndTime = EndTime;
        subtitleBehaviour.StartTime = StartTime;
        return playable;
    }
}
