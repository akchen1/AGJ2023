using DS.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;

public class DialogueClip : PlayableAsset
{
    public DSDialogueSO Dialogue;

    public override Playable CreatePlayable(PlayableGraph graph, GameObject owner)
    {
        ScriptPlayable<DialogueClipStartBehaviour> playable = ScriptPlayable<DialogueClipStartBehaviour>.Create(graph);

        DialogueClipStartBehaviour subtitleBehaviour = playable.GetBehaviour();
        subtitleBehaviour.Dialogue = Dialogue;
        return playable;
    }
}
