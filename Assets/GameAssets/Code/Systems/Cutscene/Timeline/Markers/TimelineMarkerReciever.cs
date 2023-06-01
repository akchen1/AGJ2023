using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting;
using UnityEngine;
using UnityEngine.Playables;

public class TimelineMarkerReciever : MonoBehaviour, INotificationReceiver
{
    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

    public void OnNotify(Playable origin, INotification notification, object context)
    {
        if (notification is AudioMarkerEmitter audioMarker)
        {
            RecieveAudioMarker(audioMarker);
        } else if (notification is SceneChangeMarkerEmitter sceneChangeMarker)
        {
            RecieveSceneChangeMarker(sceneChangeMarker);
        }
    }

    private void RecieveSceneChangeMarker(SceneChangeMarkerEmitter sceneChangeMarker)
    {
        eventBrokerComponent.Publish(this, new SceneEvents.SceneChange(Constants.SceneNamesArray[sceneChangeMarker.selectedSceneIndex]));
    }

    private void RecieveAudioMarker(AudioMarkerEmitter audioMarker)
    {
        if (audioMarker.IsMusic)
        {
            eventBrokerComponent.Publish(this, new AudioEvents.PlayMusic(audioMarker.AudioName));

        } else
        {
            eventBrokerComponent.Publish(this, new AudioEvents.PlaySFX(audioMarker.AudioName));
        }
    }


}
