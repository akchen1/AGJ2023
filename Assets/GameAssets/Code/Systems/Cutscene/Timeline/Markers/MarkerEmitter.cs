using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.Timeline;

public class MarkerEmitter : Marker, INotification, INotificationOptionProvider
{
    [SerializeField] private bool retroactive = false;
    [SerializeField] private bool emiteOnce = false;

    public PropertyName id => new PropertyName();

    public NotificationFlags flags => 
        (retroactive ? NotificationFlags.Retroactive : default) | 
        (emiteOnce ? NotificationFlags.TriggerOnce : default);
}
