using DS.ScriptableObjects;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class SearchScene7Controller : SceneController
{
	[Header("Testing")]
	public InventoryItem[] startingitems;

	[Header("Initialization")]
	[SerializeField] GameObject Player;
	[SerializeField] private DSDialogueSO startingDialogue;
	[SerializeField] private PlayableDirector playableDirector;

	[Header("Subscene transitions")]
	[SerializeField] private Image fadeToBlack;
	[SerializeField] private float transitionSpeedMultiplier;
	[SerializeField] private float transitionTime;


	[SerializeField, Header("Main Street")]
    private MainStreetSubSceneController MainStreetSubSceneController;

    [SerializeField, Header("Basement")]
	private BasementSubsceneController BasementSubsceneController;

	[SerializeField, Header("Forest")] private ForestSubSceneController ForestSubSceneController;


    [SerializeField, Header("Living Room")] private LivingRoomSubSceneController LivingRoomSubSceneController;


    [SerializeField, Header("General Store")] private GeneralStoreSubSceneController GeneralStoreSubSceneController;

	private SubSceneController currentSubScene;

	EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

	private void ChangeSubsceneHandler(BrokerEvent<Scene7Events.ChangeSubscene> inEvent)
	{
		Constants.Scene7SubScenes subscene = inEvent.Payload.Subscene;

		StartCoroutine(FadeBetweenCams(() =>
		{
            eventBrokerComponent.Publish(this, new InteractionEvents.InteractEnd());
            currentSubScene.Disable();
            currentSubScene = GetNextSubscene(subscene);
            currentSubScene.Enable();
        }));
	}

    private void GetBloodSanityResultHandler(BrokerEvent<Scene7Events.GetBloodSanityResult> obj)
    {
		obj.Payload.SanityType?.Invoke(BasementSubsceneController.sanityEventResult);
    }

    private SubSceneController GetNextSubscene(Constants.Scene7SubScenes subscene)
	{
		switch (subscene)
		{
			case Constants.Scene7SubScenes.MainStreet:
				return MainStreetSubSceneController;

			case Constants.Scene7SubScenes.GeneralStore:
				return GeneralStoreSubSceneController;

			case Constants.Scene7SubScenes.Basement:
				return BasementSubsceneController;

			case Constants.Scene7SubScenes.Forest:
				return ForestSubSceneController;

			case Constants.Scene7SubScenes.LivingRoom:
				return LivingRoomSubSceneController;
		}
		return null;
	}

	private IEnumerator FadeBetweenCams(Action onScreenBlack = null)
	{
		fadeToBlack.gameObject.SetActive(true);
		fadeToBlack.color = new Color(fadeToBlack.color.r, fadeToBlack.color.g, fadeToBlack.color.b, 0);

		while (fadeToBlack.color.a < 1f)
		{
			fadeToBlack.color = new Color(fadeToBlack.color.r, fadeToBlack.color.g, fadeToBlack.color.b, fadeToBlack.color.a + (Time.deltaTime * transitionSpeedMultiplier));
			yield return null;
		}

		fadeToBlack.color = new Color(fadeToBlack.color.r, fadeToBlack.color.g, fadeToBlack.color.b, 1f);

		onScreenBlack?.Invoke();
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
		eventBrokerComponent.Publish(this, new InventoryEvents.AddItem(startingitems));
		eventBrokerComponent.Publish(this, new AudioEvents.PlayMusic(Constants.Audio.Music.MainStreet, true));
		fadeToBlack.gameObject.SetActive(false);
		currentSubScene = MainStreetSubSceneController;
		currentSubScene.Enable();
		PlayStartingDialogue();
	}

    private void PlayStartingDialogue()
    {
		startingDialogue.Interact(this, Constants.Interaction.InteractionType.Virtual);
    }

    private void OnEnable()
	{
		eventBrokerComponent.Subscribe<Scene7Events.ChangeSubscene>(ChangeSubsceneHandler);
		eventBrokerComponent.Subscribe<Scene7Events.GetBloodSanityResult>(GetBloodSanityResultHandler);
	}

    private void OnDisable()
	{
		eventBrokerComponent.Unsubscribe<Scene7Events.ChangeSubscene>(ChangeSubsceneHandler);
        eventBrokerComponent.Unsubscribe<Scene7Events.GetBloodSanityResult>(GetBloodSanityResultHandler);
    }

    private void Update()
    {
        if (currentSubScene != null)
		{
			currentSubScene.Update();
		}
    }
}
