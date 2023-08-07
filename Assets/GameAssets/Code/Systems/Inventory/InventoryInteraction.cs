using DS;
using DS.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu]
public class InventoryInteraction : ScriptableObject, IInteractableVirtual
{
    [field: SerializeField] public bool RequiredItemInteraction { get; private set; } = false;
    [field: SerializeField] public List<InventoryItem> RequiredItems { get; private set; }

    [field:SerializeField] public bool OverrideDefaultClickInteraction { get; private set; } = false;

    [SerializeField] private bool playCombineSFX;

    [SerializeField] private GameObject minigame;

    [SerializeField] private DSDialogueSO invalidSceneInteractionDialogue;

    [field: SerializeField] public bool RequiredSceneInteraction { get; private set; } = false;

    [HideInInspector]
    public string selectedSceneName;
    [HideInInspector] public Constants.Scene7SubScenes selectedSubsceneScene;


    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

    public void OnDragInteraction()
    {
        if (!CheckRequirements()) return;
        Interact();
    }

    public void OnClickInteraction()
    {
        if (!CheckRequirements()) return;
        if (playCombineSFX)
            eventBrokerComponent.Publish(this, new AudioEvents.PlaySFX(Constants.Audio.SFX.ItemCombine));
        Interact();
    }

    public void Interact()
    {
        GameObject createdMinigame = Instantiate(minigame, FindObjectOfType<Canvas>().transform);
        IMinigame iMinigame = createdMinigame.GetComponent<IMinigame>();
        iMinigame.Interact(this, Constants.Interaction.InteractionType.Virtual);
    }

    private bool CheckRequirements()
    {
        string currentScene = SceneManager.GetActiveScene().name;
        if (RequiredSceneInteraction && currentScene != selectedSceneName)
        {
            PlayInvalidSceneInteractionDialogue();
            return false;
        }
        
        bool valid = true;
        if (RequiredSceneInteraction && currentScene == Constants.SceneNames.SearchScene7MainStreet)
        {
            eventBrokerComponent.Publish(this, new Scene7Events.GetCurrentSubScene(subscene =>
            {
                valid = subscene == selectedSubsceneScene;
            }));
        }
        if (!valid)
        {
            PlayInvalidSceneInteractionDialogue();
            return false;
        }

        if (minigame == null) return false;
        return true;
    }

    private void PlayInvalidSceneInteractionDialogue()
    {
        if (invalidSceneInteractionDialogue != null)
        {
            invalidSceneInteractionDialogue.Interact(this, Constants.Interaction.InteractionType.Virtual);
        }
    }
}
