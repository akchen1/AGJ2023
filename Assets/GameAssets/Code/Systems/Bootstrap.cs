using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Bootstrap : MonoBehaviour
{
    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

    //private CutsceneSystem cutsceneSystem;
    private InventorySystem inventorySystem;
    private DialogueSystem dialogueSystem;
    [SerializeField] private SanitySystem sanitySystem;
    private InteractionSystem interactionSystem;
    private PlayerInputSystem playerInputSystem;

    private SceneUtility sceneUtility;

    private void Awake()
    {
        InitializeSystem();
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

    private void InitializeSystem()
    {
        //cutsceneSystem = new CutsceneSystem();
        inventorySystem = new InventorySystem();
        interactionSystem = new InteractionSystem();
        //sanitySystem = new SanitySystem();
        dialogueSystem = new DialogueSystem();
        playerInputSystem = new PlayerInputSystem();
    }

    private void InitializeUtility()
    {
        sceneUtility = new SceneUtility(this);
    }

    private void LoadMainMenu()
    {
        eventBrokerComponent.Publish(this, new SceneEvents.SceneChange(Constants.SceneNames.MainMenu, false));
    }
}
