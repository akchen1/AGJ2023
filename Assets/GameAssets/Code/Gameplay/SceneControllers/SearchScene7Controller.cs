using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;
using Cinemachine;

public class SearchScene7Controller : SceneController
{
	[SerializeField] private PlayableDirector playableDirector;
	[SerializeField] private Image fadeToBlack;
	[SerializeField] private float transitionSpeedMultiplier;
	[SerializeField] private float transitionTime;

	[SerializeField, Header("Main Street")] CinemachineVirtualCamera mainStreetCam;
	[SerializeField] private PlayableAsset mainStreetStartingCutscene;

	[SerializeField, Header("Basement")] private CinemachineVirtualCamera basementCam;

	[SerializeField, Header("Forest")] private CinemachineVirtualCamera forestCam;

	[SerializeField, Header("Living Room")] private CinemachineVirtualCamera livingRoomCam;

	[SerializeField, Header("General Store")] private CinemachineVirtualCamera generalStoreCam;
	[SerializeField] private PlayableAsset generalStoreStartingCutscene;

	[SerializeField, Header("Playground")] private CinemachineVirtualCamera playgroundCam;

	EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

	private void ChangeSubsceneHandler(BrokerEvent<Scene7Events.ChangeSubscene> inEvent)
	{
		Constants.Scene7SubScenes subscene = inEvent.Payload.Subscene;

		StartCoroutine(FadeBetweenCams());
		HandleCameras(subscene);

		switch (inEvent.Payload.Subscene)
		{
			case Constants.Scene7SubScenes.MainStreet:
				break;

			case Constants.Scene7SubScenes.GeneralStore:
				HandleCameras(Constants.Scene7SubScenes.GeneralStore);
				playableDirector?.Play(generalStoreStartingCutscene);
				break;

			case Constants.Scene7SubScenes.Basement:
				break;

			case Constants.Scene7SubScenes.Forest:
				break;

			case Constants.Scene7SubScenes.LivingRoom:
				break;

			case Constants.Scene7SubScenes.Playground:
				break;
		}
	}

	private void HandleCameras(Constants.Scene7SubScenes subscene)
	{
		mainStreetCam.enabled = subscene == Constants.Scene7SubScenes.MainStreet;
		basementCam.enabled = subscene == Constants.Scene7SubScenes.Basement;
		forestCam.enabled = subscene == Constants.Scene7SubScenes.Forest;
		livingRoomCam.enabled = subscene == Constants.Scene7SubScenes.LivingRoom;
		generalStoreCam.enabled = subscene == Constants.Scene7SubScenes.GeneralStore;
		playgroundCam.enabled = subscene == Constants.Scene7SubScenes.Playground;
	}

	private IEnumerator FadeBetweenCams()
	{
		fadeToBlack.gameObject.SetActive(true);
		fadeToBlack.color = new Color(fadeToBlack.color.r, fadeToBlack.color.g, fadeToBlack.color.b, 0);

		while (fadeToBlack.color.a < 1f)
		{
			fadeToBlack.color = new Color(fadeToBlack.color.r, fadeToBlack.color.g, fadeToBlack.color.b, fadeToBlack.color.a + (Time.deltaTime * transitionSpeedMultiplier));
			yield return null;
		}

		fadeToBlack.color = new Color(fadeToBlack.color.r, fadeToBlack.color.g, fadeToBlack.color.b, 1f);

		yield return new WaitForSeconds(transitionTime);

		while (fadeToBlack.color.a > 0f)
		{
			fadeToBlack.color = new Color(fadeToBlack.color.r, fadeToBlack.color.g, fadeToBlack.color.b, fadeToBlack.color.a - (Time.deltaTime * transitionSpeedMultiplier));
			yield return null;
		}

		fadeToBlack.color = new Color(fadeToBlack.color.r, fadeToBlack.color.g, fadeToBlack.color.b, 0f);
		fadeToBlack.gameObject.SetActive(false);
	}

	private void Start()
	{
		HandleCameras(Constants.Scene7SubScenes.MainStreet);
		fadeToBlack.gameObject.SetActive(false);
		playableDirector?.Play(mainStreetStartingCutscene);
	}

	private void OnEnable()
	{
		eventBrokerComponent.Subscribe<Scene7Events.ChangeSubscene>(ChangeSubsceneHandler);
	}

	private void OnDisable()
	{
		eventBrokerComponent.Unsubscribe<Scene7Events.ChangeSubscene>(ChangeSubsceneHandler);
	}
}
