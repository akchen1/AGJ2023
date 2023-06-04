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
	[SerializeField] private Button resumeButton;

	private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

	private void TogglePauseHandler(BrokerEvent<PauseEvents.TogglePause> inEvent)
	{
		pausePanel.SetActive(!pausePanel.activeSelf);
	}

	private void OnResumeButtonPressed()
	{
		pausePanel.SetActive(!pausePanel.activeSelf);
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

		resumeButton.onClick.AddListener(OnResumeButtonPressed);

		sfxSlider.value = PlayerPrefs.GetFloat(Constants.Audio.SFXVolumePP, Constants.Audio.DefaultAudioLevel);
		musicSlider.value = PlayerPrefs.GetFloat(Constants.Audio.MusicVolumePP, Constants.Audio.DefaultAudioLevel);
	}

	private void OnDisable()
	{
		eventBrokerComponent.Unsubscribe<PauseEvents.TogglePause>(TogglePauseHandler);

		resumeButton.onClick.RemoveListener(OnResumeButtonPressed);

		sfxSlider.onValueChanged.RemoveListener(OnSFXSliderValueChanged);
		musicSlider.onValueChanged.RemoveListener(OnMusicSliderValueChanged);
	}
}
