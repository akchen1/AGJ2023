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

	private void TogglePauseHandler(BrokerEvent<PauseEvents.TogglePause> inEvent)
	{
		pausePanel.SetActive(!pausePanel.activeSelf);
	}

	private void OnPauseButtonPressed()
	{
		pausePanel.SetActive(!pausePanel.activeSelf);
	}

	private void OnTitleButtonPressed()
	{
		eventBrokerComponent.Publish(this, new SceneEvents.SceneChange(Constants.SceneNames.MainMenu, true));
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
		eventBrokerComponent.Subscribe<PauseEvents.TogglePause>(TogglePauseHandler);

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
		eventBrokerComponent.Unsubscribe<PauseEvents.TogglePause>(TogglePauseHandler);

		titleButton.onClick.RemoveListener(OnTitleButtonPressed);
		pauseButton.onClick.RemoveListener(OnPauseButtonPressed);
		unPauseButton.onClick.RemoveListener(OnPauseButtonPressed);

		sfxSlider.onValueChanged.RemoveListener(OnSFXSliderValueChanged);
		musicSlider.onValueChanged.RemoveListener(OnMusicSliderValueChanged);
	}
}
