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

    [SerializeField] private GameObject minigame;

    [field: SerializeField] public bool RequiredSceneInteraction { get; private set; } = false;

    [HideInInspector]
    public string selectedSceneName;


    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

    public void OnDragInteraction()
    {
        if (!CheckRequirements()) return;
        Interact();
    }

    public void OnClickInteraction()
    {
        if (!CheckRequirements()) return;
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
        if (RequiredSceneInteraction && SceneManager.GetActiveScene().name != selectedSceneName) return false;

        if (minigame == null) return false;
        return true;
    }
}
