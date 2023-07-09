using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Windows;

public class AnimatorParameterMarkerReciever : MonoBehaviour, INotificationReceiver
{
    private Animator animator;

    private void Awake()
    {
        animator = GetComponent<Animator>();
    }

    public void OnNotify(Playable origin, INotification notification, object context)
    {
        if (notification is AnimatorParameterMarkerEmitter animMarker)
        {
            RecieveAnimatorParameterMarker(animMarker);
        }
    }

    private void RecieveAnimatorParameterMarker(AnimatorParameterMarkerEmitter animMarker)
    {
        foreach (AnimationParameter animationParameter in animMarker.ParameterValues)
        {
            switch (animationParameter.ParameterType)
            {
                case AnimatorControllerParameterType.Float:
                    animator.SetFloat(animationParameter.ParameterName, animationParameter.ParameterFloatValue);
                    break;
                case AnimatorControllerParameterType.Bool:
                    animator.SetBool(animationParameter.ParameterName, animationParameter.ParameterBoolValue);
                    break;
                case AnimatorControllerParameterType.Int:
                    animator.SetInteger(animationParameter.ParameterName, animationParameter.ParameterIntValue);
                    break;
                case AnimatorControllerParameterType.Trigger:
                    if (animationParameter.ParameterBoolValue)
                    {
                        animator.SetTrigger(animationParameter.ParameterName);
                    }
                    else
                    {
                        animator.ResetTrigger(animationParameter.ParameterName);
                    }

                    break;
            }
        }
    }
}
