using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AudioSystem
{
	private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

	public AudioSystem()
    {
        eventBrokerComponent.Subscribe<AudioEvents.PlayMusic>(PlayMusicHandler);
        eventBrokerComponent.Subscribe<AudioEvents.PlaySFX>(PlaySFXHandler);
    }

    ~AudioSystem()
    {
        eventBrokerComponent.Unsubscribe<AudioEvents.PlayMusic>(PlayMusicHandler);
        eventBrokerComponent.Unsubscribe<AudioEvents.PlaySFX>(PlaySFXHandler);
    }

    private void PlayMusicHandler(BrokerEvent<AudioEvents.PlayMusic> inEvent)
    {
		// TODO: Play music
        Debug.Log("Play music " + inEvent.Payload.MusicName);
    }

    private void PlaySFXHandler(BrokerEvent<AudioEvents.PlaySFX> inEvent)
    {
		// TODO: Play SFX
        Debug.Log("Play SFX " + inEvent.Payload.SFXName);
    }
}

