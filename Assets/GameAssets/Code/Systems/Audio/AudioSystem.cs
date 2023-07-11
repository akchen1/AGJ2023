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
	[SerializeField] private AudioClip basement;
	[SerializeField] private AudioClip generalStore;
	[SerializeField] private AudioClip mainStreet;
	[SerializeField] private AudioClip playtime;

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
	[SerializeField] private AudioClip pocketKnifeCollect;
	[SerializeField] private AudioClip clayCollect;
	[SerializeField] private AudioClip dig;
	[SerializeField] private AudioClip digBarHit;
	[SerializeField] private AudioClip matchLight;
	[SerializeField] private AudioClip matchStrike;
	[SerializeField] private AudioClip matchboxCollect;
	[SerializeField] private AudioClip matchboxOpen;
	[SerializeField] private AudioClip shovelCollect;
	[SerializeField] private AudioClip twineCollect;
	[SerializeField] private AudioClip vaseShatter;
	[SerializeField] private AudioClip vialCollect;
	[SerializeField] private AudioClip branchBreak;
	[SerializeField] private AudioClip branchCollect;
	[SerializeField] private AudioClip gemCollect;
	[SerializeField] private AudioClip dialTurn;
	[SerializeField] private AudioClip boxOpen;
	[SerializeField] private AudioClip bottleHit;
	[SerializeField] private AudioClip broomCrash;
	[SerializeField] private AudioClip shears;
	[SerializeField] private AudioClip toyCarRoll;
	[SerializeField] private AudioClip bloodDrop;
	[SerializeField] private AudioClip itemCombine;

	private float musicVolume;
	private float sfxVolume;

	private AudioClip oldMusic;
	private float oldTime;

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
		music.Add(Constants.Audio.Music.Basement, basement);
		music.Add(Constants.Audio.Music.GeneralStore, generalStore);
		music.Add(Constants.Audio.Music.MainStreet, mainStreet);
		music.Add(Constants.Audio.Music.Playtime, playtime);

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
		sfx.Add(Constants.Audio.SFX.PocketKnifeCollect, pocketKnifeCollect);
		sfx.Add(Constants.Audio.SFX.ClayCollect, clayCollect);
		sfx.Add(Constants.Audio.SFX.Dig, dig);
		sfx.Add(Constants.Audio.SFX.DigBarHit, digBarHit);
		sfx.Add(Constants.Audio.SFX.MatchLight, matchLight);
		sfx.Add(Constants.Audio.SFX.MatchStrike, matchStrike);
		sfx.Add(Constants.Audio.SFX.MatchboxCollect, matchboxCollect);
		sfx.Add(Constants.Audio.SFX.MatchboxOpen, matchboxOpen);
		sfx.Add(Constants.Audio.SFX.ShovelCollect, shovelCollect);
		sfx.Add(Constants.Audio.SFX.TwineCollect, twineCollect);
		sfx.Add(Constants.Audio.SFX.VaseShatter, vaseShatter);
		sfx.Add(Constants.Audio.SFX.VialCollect, vialCollect);
		sfx.Add(Constants.Audio.SFX.BranchBreak, branchBreak);
		sfx.Add(Constants.Audio.SFX.BranchCollect, branchCollect);
		sfx.Add(Constants.Audio.SFX.GemCollect, gemCollect);
		sfx.Add(Constants.Audio.SFX.DialTurn, dialTurn);
		sfx.Add(Constants.Audio.SFX.BoxOpen, boxOpen);
		sfx.Add(Constants.Audio.SFX.BottleHit, bottleHit);
		sfx.Add(Constants.Audio.SFX.BroomCrash, broomCrash);
		sfx.Add(Constants.Audio.SFX.Shears, shears);
		sfx.Add(Constants.Audio.SFX.ToyCarRoll, toyCarRoll);
		sfx.Add(Constants.Audio.SFX.BloodDrop, bloodDrop);
		sfx.Add(Constants.Audio.SFX.ItemCombine, itemCombine); 
	}

	private void OnEnable()
    {
        eventBrokerComponent.Subscribe<AudioEvents.PlayMusic>(PlayMusicHandler);
        eventBrokerComponent.Subscribe<AudioEvents.PlaySFX>(PlaySFXHandler);
		eventBrokerComponent.Subscribe<AudioEvents.ChangeMusicVolume>(ChangeMusicVolumeHandler);
		eventBrokerComponent.Subscribe<AudioEvents.ChangeSFXVolume>(ChangeSFXVolumeHandler);
		eventBrokerComponent.Subscribe<AudioEvents.PlayTemporaryMusic>(PlayTemporaryMusicHandler);
		eventBrokerComponent.Subscribe<AudioEvents.StopTemporaryMusic>(StopTemporaryMusicHandler);

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
		eventBrokerComponent.Unsubscribe<AudioEvents.PlayTemporaryMusic>(PlayTemporaryMusicHandler);
		eventBrokerComponent.Unsubscribe<AudioEvents.StopTemporaryMusic>(StopTemporaryMusicHandler);
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

	private void PlayTemporaryMusicHandler(BrokerEvent<AudioEvents.PlayTemporaryMusic> inEvent)
	{
		oldMusic = musicSource.clip;
		oldTime = musicSource.time;
		StartCoroutine(FadeToSong(inEvent.Payload.MusicName));
	}

	private void StopTemporaryMusicHandler(BrokerEvent<AudioEvents.StopTemporaryMusic> inEvent)
	{
		StartCoroutine(FadeToSong(oldMusic, oldTime));
	}

	private void PlayMusic(string song, float time = 0f)
	{
		if (music.ContainsKey(song))
		{
			musicSource.Stop();
			musicSource.clip = music[song];
			musicSource.loop = true;
			musicSource.Play();
			musicSource.time = time;
		}
		else
		{
			Debug.LogError("Cannot find music named " + song);
		}
	}

	private void PlayMusic(AudioClip song, float time = 0f)
	{
		musicSource.Stop();
		musicSource.clip = song;
		musicSource.loop = true;
		musicSource.Play();
		musicSource.time = time;
	}

	private IEnumerator FadeToSong(string song, float time = 0f)
	{
		while (musicSource.volume > 0)
		{
			musicSource.volume -= Constants.Audio.MusicFadeSpeed * Time.deltaTime;
			yield return null;
		}

		PlayMusic(song, time);

		while (musicSource.volume < musicVolume)
		{
			musicSource.volume += Constants.Audio.MusicFadeSpeed * Time.deltaTime;
			yield return null;
		}
	}

	private IEnumerator FadeToSong(AudioClip song, float time = 0f)
	{
		while (musicSource.volume > 0)
		{
			musicSource.volume -= Constants.Audio.MusicFadeSpeed * Time.deltaTime;
			yield return null;
		}

		PlayMusic(song, time);

		while (musicSource.volume < musicVolume)
		{
			musicSource.volume += Constants.Audio.MusicFadeSpeed * Time.deltaTime;
			yield return null;
		}
	}
}

