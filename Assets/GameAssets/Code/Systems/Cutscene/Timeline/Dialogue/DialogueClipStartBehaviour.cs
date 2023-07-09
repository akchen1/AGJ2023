using DS.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using TMPro;
using UnityEngine;
using UnityEngine.Playables;

public class DialogueClipStartBehaviour : PlayableBehaviour
{
    public DSDialogueSO Dialogue;
    public bool canSkipDialogue;
    public bool waitForPlayerInput;

    public double StartTime;
    public double EndTime;
    public double EaseInTime;
    public double EaseOutTime;
}
