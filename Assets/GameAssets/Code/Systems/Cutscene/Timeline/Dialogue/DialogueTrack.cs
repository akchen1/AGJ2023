using DS.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

[TrackBindingType(typeof(TimelineDialogueSystem))]
[TrackClipType(typeof(DialogueClip))]
public class DialogueTrack : TrackAsset
{
    public override Playable CreateTrackMixer(PlayableGraph graph, GameObject go, int inputCount)
    {

        foreach (TimelineClip clip in GetClips())
        {
            DialogueClip dialogueClip = clip.asset as DialogueClip;
            if (dialogueClip != null)
            {
                dialogueClip.StartTime = clip.start;
                dialogueClip.EndTime = clip.end;
                dialogueClip.EaseInTime = clip.easeInDuration;
                dialogueClip.EaseOutTime = clip.easeOutDuration;
            }
        }
        return ScriptPlayable<DialogueTrackMixer>.Create(graph, inputCount);
    }
}
