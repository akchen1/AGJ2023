using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrap : MonoBehaviour
{
    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

    [SerializeField] private CutsceneSystem cutsceneSystem;
    [SerializeField] private DialogueSystem dialogueSystem;
    [SerializeField] private InventorySystem inventorySystem;
    [SerializeField] private SanitySystem sanitySystem;
    private DataPersistenceSystem dataPersistenceSystem;
    private SceneUtility sceneUtility;

    private void Awake()
    {
        DontDestroyOnLoad(this);
        InitializeUtility();
    }

    private void Start()
    {
#if !UNITY_EDITOR
        // Because we are using Main Scene Editor Utility and we don't want to load MainMenu if we are on another
        // scene than Bootstrap. For production, Main menu will always be loaded.
        LoadMainMenu();
#endif
    }

    private void InitializeUtility()
    {
        sceneUtility = new SceneUtility(this);
        dataPersistenceSystem = new DataPersistenceSystem();
    }

    private void LoadMainMenu()
    {
		eventBrokerComponent.Publish(this, new SceneEvents.SceneChange(Constants.SceneNames.MainMenu, false));
    }
}
