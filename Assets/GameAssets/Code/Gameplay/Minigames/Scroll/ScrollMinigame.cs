using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ScrollMinigame : ObjectPlacementMinigame
{
    [SerializeField] private Animator scrollFinishAnimator;
    protected override IEnumerator DelayedFinish()
    {
        scrollFinishAnimator.SetTrigger("FadeIn");
        return base.DelayedFinish();
    }
}
