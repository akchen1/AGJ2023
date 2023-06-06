using DS;
using DS.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

[CreateAssetMenu]
public class InventoryInteraction : ScriptableObject, IInteractable
{
    [field: SerializeField] public bool RequiredItemInteraction { get; private set; } = false;
    [field: SerializeField] public List<InventoryItem> RequiredItems { get; private set; }

    [SerializeField] private GameObject minigame;
    [field: SerializeField] public bool RequiredSceneInteraction { get; private set; } = false;
    [HideInInspector]
    public string selectedSceneName;


    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

    public void OnDragInteraction()
    {
        if (RequiredSceneInteraction && SceneManager.GetActiveScene().name != selectedSceneName) return;
        
        if (minigame != null)
        {
            eventBrokerComponent.Publish(this, new InteractionEvents.Interact(this, valid =>
            {
                if (valid)
                {
                    Interact();
                }
            }));
        }
    }

    public void Interact()
    {
        GameObject createdMinigame = Instantiate(minigame, FindObjectOfType<Canvas>().transform);
        eventBrokerComponent.Publish(this, new MinigameEvents.StartMinigame(createdMinigame.GetComponent<IMinigame>()));
    }
}
