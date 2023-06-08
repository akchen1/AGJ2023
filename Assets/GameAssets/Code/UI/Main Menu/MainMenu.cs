using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class MainMenu : MonoBehaviour
{
	[SerializeField] private Button startButton;
	[SerializeField] private Button creditsButton;

	[SerializeField, Header("Credits")] private GameObject creditsPanel;
	[SerializeField] private Button backButton;

	private EventBrokerComponent eventBroker = new EventBrokerComponent();

	private void OnStartButtonPressed()
	{
		eventBroker.Publish(this, new SceneEvents.SceneChange(Constants.SceneNames.PrologueScene1));
	}

	private void OnCreditsButtonPressed()
	{
		creditsPanel.SetActive(true);
	}

	private void OnBackButtonPressed()
	{
		creditsPanel.SetActive(false);
	}

	private void OnEnable()
	{
		startButton.onClick.AddListener(OnStartButtonPressed);
		creditsButton.onClick.AddListener(OnCreditsButtonPressed);
		backButton.onClick.AddListener(OnBackButtonPressed);
	}

	private void OnDisable()
	{
		startButton.onClick.RemoveListener(OnStartButtonPressed);
		creditsButton.onClick.RemoveListener(OnCreditsButtonPressed);
		backButton.onClick.RemoveListener(OnBackButtonPressed);
	}
}
