using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AudioSystem : MonoBehaviour
{
	[SerializeField] private AudioSource musicSource;
	[SerializeField] private AudioSource sfxSource;

	private float musicVolume;
	private float sfxVolume;

	private Dictionary<string, AudioClip> music = new Dictionary<string, AudioClip>();
	private Dictionary<string, AudioClip> sfx = new Dictionary<string, AudioClip>();

	private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

	private void OnEnable()
    {
        eventBrokerComponent.Subscribe<AudioEvents.PlayMusic>(PlayMusicHandler);
        eventBrokerComponent.Subscribe<AudioEvents.PlaySFX>(PlaySFXHandler);
		eventBrokerComponent.Subscribe<AudioEvents.ChangeMusicVolume>(ChangeMusicVolumeHandler);
		eventBrokerComponent.Subscribe<AudioEvents.ChangeSFXVolume>(ChangeSFXVolumeHandler);

		float musicLevel = PlayerPrefs.GetFloat(Constants.Audio.MusicVolumePP, Constants.Audio.DefaultAudioLevel);
		float sfxLevel = PlayerPrefs.GetFloat(Constants.Audio.SFXVolumePP, Constants.Audio.DefaultAudioLevel);

		musicVolume = musicLevel;
		sfxVolume = sfxLevel;
		musicSource.volume = musicLevel;
		sfxSource.volume = sfxLevel;
	}

    private void OnDisable()
    {
        eventBrokerComponent.Unsubscribe<AudioEvents.PlayMusic>(PlayMusicHandler);
        eventBrokerComponent.Unsubscribe<AudioEvents.PlaySFX>(PlaySFXHandler);
		eventBrokerComponent.Unsubscribe<AudioEvents.ChangeMusicVolume>(ChangeMusicVolumeHandler);
		eventBrokerComponent.Unsubscribe<AudioEvents.ChangeSFXVolume>(ChangeSFXVolumeHandler);
	}

	private void ChangeMusicVolumeHandler(BrokerEvent<AudioEvents.ChangeMusicVolume> inEvent)
	{
		musicVolume = inEvent.Payload.NewVolume;
		musicSource.volume = musicVolume;

		PlayerPrefs.SetFloat(Constants.Audio.MusicVolumePP, musicVolume);
	}

	private void ChangeSFXVolumeHandler(BrokerEvent<AudioEvents.ChangeSFXVolume> inEvent)
	{
		sfxVolume = inEvent.Payload.NewVolume;
		sfxSource.volume = sfxVolume;

		PlayerPrefs.SetFloat(Constants.Audio.SFXVolumePP, musicVolume);
	}

	private void PlayMusicHandler(BrokerEvent<AudioEvents.PlayMusic> inEvent)
    {
		if (inEvent.Payload.Transition)
		{
			StartCoroutine(FadeToSong(inEvent.Payload.MusicName));
		}
		else
		{
			PlayMusic(inEvent.Payload.MusicName);
		}
    }

    private void PlaySFXHandler(BrokerEvent<AudioEvents.PlaySFX> inEvent)
    {
		if (sfx.ContainsKey(inEvent.Payload.SFXName))
		{
			sfxSource.PlayOneShot(sfx[inEvent.Payload.SFXName]);
		}
		else
		{
			Debug.LogError("Cannot find sfx named " + inEvent.Payload.SFXName);
		}
    }

	private void PlayMusic(string song)
	{
		if (music.ContainsKey(song))
		{
			musicSource.Stop();
			musicSource.clip = music[song];
			musicSource.loop = true;
			musicSource.Play();
		}
		else
		{
			Debug.LogError("Cannot find music named " + song);
		}
	}

	private IEnumerator FadeToSong(string song)
	{
		while (musicSource.volume > 0)
		{
			musicSource.volume -= Constants.Audio.MusicFadeSpeed * Time.deltaTime;
			yield return null;
		}

		PlayMusic(song);

		while (musicSource.volume < musicVolume)
		{
			musicSource.volume += Constants.Audio.MusicFadeSpeed * Time.deltaTime;
			yield return null;
		}
	}
}

