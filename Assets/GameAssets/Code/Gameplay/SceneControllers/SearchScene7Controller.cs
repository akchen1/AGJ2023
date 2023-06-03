using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using Cinemachine;

public class SearchScene7Controller : SceneController
{
	[SerializeField] private PlayableDirector playableDirector;

	[SerializeField, Header("Main Street")] CinemachineVirtualCamera mainStreetCam;
	[SerializeField] private PlayableAsset mainStreetStartingCutscene;

	[SerializeField, Header("Basement")] private CinemachineVirtualCamera basementCam;

	[SerializeField, Header("Forest")] private CinemachineVirtualCamera forestCam;

	[SerializeField, Header("Living Room")] private CinemachineVirtualCamera livingRoomCam;

	[SerializeField, Header("General Store")] private CinemachineVirtualCamera generalStoreCam;

	[SerializeField, Header("Playground")] private CinemachineVirtualCamera playgroundCam;

	EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

	private void ChangeSubsceneHandler(BrokerEvent<Scene7Events.ChangeSubscene> inEvent)
	{
		Constants.Scene7SubScenes subscene = inEvent.Payload.Subscene;

		mainStreetCam.enabled = subscene == Constants.Scene7SubScenes.MainStreet;
		basementCam.enabled = subscene == Constants.Scene7SubScenes.Basement;
		forestCam.enabled = subscene == Constants.Scene7SubScenes.Forest;
		livingRoomCam.enabled = subscene == Constants.Scene7SubScenes.LivingRoom;
		generalStoreCam.enabled = subscene == Constants.Scene7SubScenes.GeneralStore;
		playgroundCam.enabled = subscene == Constants.Scene7SubScenes.Playground;

		switch (inEvent.Payload.Subscene)
		{
			case Constants.Scene7SubScenes.MainStreet:
				break;

			case Constants.Scene7SubScenes.GeneralStore:
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

	private void Start()
	{
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
