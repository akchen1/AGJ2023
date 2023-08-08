using DS.ScriptableObjects;
using Pathfinding;
using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class SearchScene7Controller : SceneController
{
#if UNITY_EDITOR
	[Header("Testing")]
	public InventoryItem[] startingitems;
#endif

	[Header("Initialization")]
	[SerializeField] GameObject Player;
	[SerializeField] private PlayableDirector playableDirector;
	[SerializeField] private DialogueInteraction mainStreetMaeve;
	[SerializeField] private DialogueInteraction mainStreetBadu;

	[Header("Subscene transitions")]
	[SerializeField] private Image fadeToBlack;
	[SerializeField] private float transitionSpeedMultiplier;
	[SerializeField] private float transitionTime;

    [Header("Ritual state")]
    [SerializeField] private List<CombinedRitualItem> combinedRitualItems;
    [Tooltip("Dialogue to trigger when all ritual components are obtained")]
    [SerializeField] private DSDialogueSO uncombinedDialogue;
    [Tooltip("Dialogue to trigger when ritual components are combined")]
    [SerializeField] private DSDialogueSO combinedDialogue;
    [SerializeField] private ScrollStateReference scrollStateReference;

    private bool componentsObtained;
    private bool itemsObtained;

    private static bool hasTriggeredComponentsObtainedDialogue = false;
    private static bool hasTriggeredItemsObtainedDialogue = false;

    [SerializeField, Header("Main Street")]
    private MainStreetSubSceneController MainStreetSubSceneController;

    [SerializeField, Header("Basement")]
	private BasementSubsceneController BasementSubsceneController;

	[SerializeField, Header("Forest")] private ForestSubSceneController ForestSubSceneController;


    [SerializeField, Header("Living Room")] private LivingRoomSubSceneController LivingRoomSubSceneController;


    [SerializeField, Header("General Store")] private GeneralStoreSubSceneController GeneralStoreSubSceneController;

    [SerializeField, Header("Playground")] private PlaygroundSubSceneController PlaygroundSubSceneController;
	[SerializeField] private Collider2D playgroundBounds;

    private SubSceneController currentSubScene;

	EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();


    private void Start()
	{
#if UNITY_EDITOR
		eventBrokerComponent.Publish(this, new InventoryEvents.AddItem(startingitems));
#endif
        eventBrokerComponent.Publish(this, new PostProcessingEvents.SetVignette(0.2f));

        fadeToBlack.gameObject.SetActive(false);
		currentSubScene = BasementSubsceneController;
		currentSubScene.Enable();
		//PlayStartingDialogue();
	}

    private void OnEnable()
	{
		eventBrokerComponent.Subscribe<Scene7Events.ChangeSubscene>(ChangeSubsceneHandler);
		eventBrokerComponent.Subscribe<Scene7Events.GetBloodSanityResult>(GetBloodSanityResultHandler);
		eventBrokerComponent.Subscribe<Scene7Events.GetCurrentSubScene>(GetCurrentSubSceneHandler);
		eventBrokerComponent.Subscribe<InteractionEvents.InteractEnd>(InteractEndHandler);
        eventBrokerComponent.Subscribe<Scene7Events.SetBaduSubsceneDialogue>(SetBaduSubsceneDialogueHandler);
        eventBrokerComponent.Subscribe<Scene7Events.SetMaeveSubsceneDialogue>(SetMaeveSubsceneDialogueHandler);
        eventBrokerComponent.Subscribe<Scene7Events.SetMaevePosition>(SetMaevePositionHandler);
        eventBrokerComponent.Subscribe<Scene7Events.SetBaduPosition>(SetBaduPositionHandler);
        eventBrokerComponent.Subscribe<Scene7Events.EnableBaduMovement>(EnableBaduMovementHandler);
        eventBrokerComponent.Subscribe<Scene7Events.EnableMaeveMovement>(EnableMaeveMovementHandler);
    }

    private void OnDisable()
	{
		eventBrokerComponent.Unsubscribe<Scene7Events.ChangeSubscene>(ChangeSubsceneHandler);
        eventBrokerComponent.Unsubscribe<Scene7Events.GetBloodSanityResult>(GetBloodSanityResultHandler);
		eventBrokerComponent.Unsubscribe<Scene7Events.GetCurrentSubScene>(GetCurrentSubSceneHandler);
        eventBrokerComponent.Unsubscribe<InteractionEvents.InteractEnd>(InteractEndHandler);
        eventBrokerComponent.Unsubscribe<Scene7Events.SetBaduSubsceneDialogue>(SetBaduSubsceneDialogueHandler);
        eventBrokerComponent.Unsubscribe<Scene7Events.SetMaeveSubsceneDialogue>(SetMaeveSubsceneDialogueHandler);
        eventBrokerComponent.Unsubscribe<Scene7Events.SetMaevePosition>(SetMaevePositionHandler);
        eventBrokerComponent.Unsubscribe<Scene7Events.SetBaduPosition>(SetBaduPositionHandler);
        eventBrokerComponent.Unsubscribe<Scene7Events.EnableBaduMovement>(EnableBaduMovementHandler);
        eventBrokerComponent.Unsubscribe<Scene7Events.EnableMaeveMovement>(EnableMaeveMovementHandler);
    }


    private void Update()
    {
        if (currentSubScene != null)
		{
			currentSubScene.Update();
		}
		CheckInPlayground();
    }


    #region Event Methods
    private void EnableBaduMovementHandler(BrokerEvent<Scene7Events.EnableBaduMovement> inEvent)
    {
        mainStreetBadu.GetComponent<AIPath>().canMove = inEvent.Payload.Enable;
    }

    private void EnableMaeveMovementHandler(BrokerEvent<Scene7Events.EnableMaeveMovement> inEvent)
    {
        mainStreetMaeve.GetComponent<AIPath>().canMove = inEvent.Payload.Enable;
    }
    private void SetMaevePositionHandler(BrokerEvent<Scene7Events.SetMaevePosition> inEvent)
    {
        mainStreetMaeve.transform.position = inEvent.Payload.Position;
    }

    private void SetBaduPositionHandler(BrokerEvent<Scene7Events.SetBaduPosition> inEvent)
    {
        mainStreetBadu.transform.position = inEvent.Payload.Position;
    }
    private void SetMaeveSubsceneDialogueHandler(BrokerEvent<Scene7Events.SetMaeveSubsceneDialogue> inEvent)
    {
        mainStreetMaeve.SetDialogue(inEvent.Payload.Dialogue);
    }

    private void SetBaduSubsceneDialogueHandler(BrokerEvent<Scene7Events.SetBaduSubsceneDialogue> inEvent)
    {
        mainStreetBadu.SetDialogue(inEvent.Payload.Dialogue);
    }

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
    private void InteractEndHandler(BrokerEvent<InteractionEvents.InteractEnd> obj)
    {
		// Let other interactions take priorty
		Invoke("CheckScrollDialogues", 0.1f);
    }
    private void GetCurrentSubSceneHandler(BrokerEvent<Scene7Events.GetCurrentSubScene> obj)
    {
		obj.Payload.Subscene?.Invoke(currentSubScene.Subscene);
    }

    #endregion

    #region Utility Methods

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
			case Constants.Scene7SubScenes.Playground:
				return PlaygroundSubSceneController;
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

    private void CheckInPlayground()
    {
        bool inPlayground = playgroundBounds.OverlapPoint(Player.transform.position);
        if (inPlayground && currentSubScene == MainStreetSubSceneController)
        {
            currentSubScene.Disable();
            currentSubScene = GetNextSubscene(Constants.Scene7SubScenes.Playground);
            currentSubScene.Enable(true);
        }
        else if (!inPlayground && currentSubScene == PlaygroundSubSceneController)
        {
            currentSubScene.Disable();
            currentSubScene = GetNextSubscene(Constants.Scene7SubScenes.MainStreet);
            currentSubScene.Enable(true);
        }
    }

    private void CheckScrollDialogues()
    {
        itemsObtained = true;
        componentsObtained = true;
        foreach (CombinedRitualItem item in combinedRitualItems)
        {
            if (item.Item.CheckInInventory(this)) continue;
            itemsObtained = false;
            if (item.Components.Count > 0 && item.Components.CheckInInventory(this)) continue;
            componentsObtained = false;
        }
        if (itemsObtained && !hasTriggeredItemsObtainedDialogue && combinedDialogue.Interact(this, Constants.Interaction.InteractionType.Virtual))
        {
            scrollStateReference.Variable.SetValue(ScrollState.ItemsObtained);
            hasTriggeredItemsObtainedDialogue = true;
        }
        else if (!itemsObtained && componentsObtained && !hasTriggeredComponentsObtainedDialogue && uncombinedDialogue.Interact(this, Constants.Interaction.InteractionType.Virtual))
        {
            scrollStateReference.Variable.SetValue(ScrollState.ComponentsObtained);
            hasTriggeredComponentsObtainedDialogue = true;
        }
    }
    #endregion

    [System.Serializable]
    public struct CombinedRitualItem
    {
        public InventoryItem Item;
        public List<InventoryItem> Components;
    }
}
