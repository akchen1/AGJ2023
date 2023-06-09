using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class AudioSystem : MonoBehaviour
{
	[SerializeField] private AudioSource musicSource;
	[SerializeField] private AudioSource sfxSource;

	[SerializeField, Header("Music")] private AudioClip mainMenuTrack;
	[SerializeField] private AudioClip clearing;
	[SerializeField] private AudioClip endingTree;
	[SerializeField] private AudioClip forest;
	[SerializeField] private AudioClip livingRoom;
	[SerializeField] private AudioClip prologue;
	[SerializeField] private AudioClip recordPlayer;


	[SerializeField, Header("SFX")] private AudioClip click;
	[SerializeField] private AudioClip pieceLift;
	[SerializeField] private AudioClip piecePlace;
	[SerializeField] private AudioClip pieceRotate;
	[SerializeField] private AudioClip scrollClose;
	[SerializeField] private AudioClip scrollFlip;
	[SerializeField] private AudioClip scrollOpen;
	[SerializeField] private AudioClip drawerOpen;
	[SerializeField] private AudioClip drawerClose;
	[SerializeField] private AudioClip bucketCollect;
	[SerializeField] private AudioClip candleCollect;
	[SerializeField] private AudioClip dig;
	[SerializeField] private AudioClip matchLight;
	[SerializeField] private AudioClip matchStrike;
	[SerializeField] private AudioClip matchboxCollect;
	[SerializeField] private AudioClip matchboxOpen;
	[SerializeField] private AudioClip shovelCollect;
	[SerializeField] private AudioClip twineCollect;
	[SerializeField] private AudioClip vaseShatter;
	[SerializeField] private AudioClip vialCollect;

	private float musicVolume;
	private float sfxVolume;

	private Dictionary<string, AudioClip> music = new Dictionary<string, AudioClip>();
	private Dictionary<string, AudioClip> sfx = new Dictionary<string, AudioClip>();

	private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

	private void Awake()
	{
		// Set up music and sfx dictionaries
		music.Add(Constants.Audio.Music.MainMenu, mainMenuTrack);
		music.Add(Constants.Audio.Music.Clearing, clearing);
		music.Add(Constants.Audio.Music.EndingTree, endingTree);
		music.Add(Constants.Audio.Music.Forest, forest);
		music.Add(Constants.Audio.Music.LivingRoom, livingRoom);
		music.Add(Constants.Audio.Music.Prologue, prologue);
		music.Add(Constants.Audio.Music.RecordPlayer, recordPlayer);

		sfx.Add(Constants.Audio.SFX.Click, click);
		sfx.Add(Constants.Audio.SFX.PieceLift, pieceLift);
		sfx.Add(Constants.Audio.SFX.PiecePlace, piecePlace);
		sfx.Add(Constants.Audio.SFX.PieceRotate, pieceRotate);
		sfx.Add(Constants.Audio.SFX.ScrollClose, scrollClose);
		sfx.Add(Constants.Audio.SFX.ScrollFlip, scrollFlip);
		sfx.Add(Constants.Audio.SFX.ScrollOpen, scrollOpen);
		sfx.Add(Constants.Audio.SFX.DrawerOpen, drawerOpen);
		sfx.Add(Constants.Audio.SFX.DrawerClose, drawerClose);
		sfx.Add(Constants.Audio.SFX.BucketCollect, bucketCollect);
		sfx.Add(Constants.Audio.SFX.CandleCollect, candleCollect);
		sfx.Add(Constants.Audio.SFX.Dig, dig);
		sfx.Add(Constants.Audio.SFX.MatchLight, matchLight);
		sfx.Add(Constants.Audio.SFX.MatchStrike, matchStrike);
		sfx.Add(Constants.Audio.SFX.MatchboxCollect, matchboxCollect);
		sfx.Add(Constants.Audio.SFX.MatchboxOpen, matchboxOpen);
		sfx.Add(Constants.Audio.SFX.ShovelCollect, shovelCollect);
		sfx.Add(Constants.Audio.SFX.TwineCollect, twineCollect);
		sfx.Add(Constants.Audio.SFX.VaseShatter, vaseShatter);
		sfx.Add(Constants.Audio.SFX.VialCollect, vialCollect);
	}

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

