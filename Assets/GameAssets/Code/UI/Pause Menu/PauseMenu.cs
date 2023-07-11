using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using TMPro;

public class PauseMenu : MonoBehaviour
{
	[SerializeField] private GameObject pausePanel;
	[SerializeField] private Slider sfxSlider;
	[SerializeField] private Slider musicSlider;
	[SerializeField] private Button titleButton;
	[SerializeField] private Button pauseButton;
	[SerializeField] private Button unPauseButton;

	private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

	private void RestartGameHandler(BrokerEvent<GameStateEvents.RestartGame> inEvnet)
	{
		pausePanel.SetActive(false);
	}

	private void OnPauseButtonPressed()
	{
		pausePanel.SetActive(!pausePanel.activeSelf);
		eventBrokerComponent.Publish(this, new PauseEvents.TogglePause());
	}

	private void OnTitleButtonPressed()
	{
		eventBrokerComponent.Publish(this, new SceneEvents.SceneChange(Constants.SceneNames.MainMenu, true));
		eventBrokerComponent.Publish(this, new GameStateEvents.RestartGame());
	}

	private void OnMusicSliderValueChanged(float value)
	{
		eventBrokerComponent.Publish(this, new AudioEvents.ChangeMusicVolume(value));
	}

	private void OnSFXSliderValueChanged(float value)
	{
		eventBrokerComponent.Publish(this, new AudioEvents.ChangeSFXVolume(value));
	}

	private void OnEnable()
	{
		eventBrokerComponent.Subscribe<GameStateEvents.RestartGame>(RestartGameHandler);

		sfxSlider.onValueChanged.AddListener(OnSFXSliderValueChanged);
		musicSlider.onValueChanged.AddListener(OnMusicSliderValueChanged);

		titleButton.onClick.AddListener(OnTitleButtonPressed);
		pauseButton.onClick.AddListener(OnPauseButtonPressed);
		unPauseButton.onClick.AddListener(OnPauseButtonPressed);

		sfxSlider.value = PlayerPrefs.GetFloat(Constants.Audio.SFXVolumePP, Constants.Audio.DefaultAudioLevel);
		musicSlider.value = PlayerPrefs.GetFloat(Constants.Audio.MusicVolumePP, Constants.Audio.DefaultAudioLevel);
	}

	private void OnDisable()
	{
		eventBrokerComponent.Unsubscribe<GameStateEvents.RestartGame>(RestartGameHandler);

		titleButton.onClick.RemoveListener(OnTitleButtonPressed);
		pauseButton.onClick.RemoveListener(OnPauseButtonPressed);
		unPauseButton.onClick.RemoveListener(OnPauseButtonPressed);

		sfxSlider.onValueChanged.RemoveListener(OnSFXSliderValueChanged);
		musicSlider.onValueChanged.RemoveListener(OnMusicSliderValueChanged);
	}
}
