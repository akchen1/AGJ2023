using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AudioSystem
{
    public AudioSystem()
    {
        Bootstrap.EventBrokerComponent.Subscribe<Event.PlayMusic>(PlayMusicHandler);
        Bootstrap.EventBrokerComponent.Subscribe<Event.PlaySFX>(PlaySFXHandler);
    }

    ~AudioSystem()
    {
        Bootstrap.EventBrokerComponent.Unsubscribe<Event.PlayMusic>(PlayMusicHandler);
        Bootstrap.EventBrokerComponent.Unsubscribe<Event.PlaySFX>(PlaySFXHandler);
    }

    private void PlayMusicHandler(BrokerEvent<Event.PlayMusic> inEvent)
    {
        Debug.Log("Play music " + inEvent.Payload.MusicName);
    }

    private void PlaySFXHandler(BrokerEvent<Event.PlaySFX> inEvent)
    {
        Debug.Log("Play SFX " + inEvent.Payload.SFXName);
    }
}

