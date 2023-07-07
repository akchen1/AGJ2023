using System.Collections;
using System.Collections.Generic;
using Unity.Burst.CompilerServices;
using UnityEngine;

public class RequiredItemsMinigame : MonoBehaviour, IMinigame
{
    [SerializeField] private GameObject minigameUI;

    [SerializeField] private List<InventoryItem> requiredItems;

    private bool active = false;
    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();
    public void Finish()
    {
        active = false;
        minigameUI.SetActive(false);

        this.EndMinigame();
        eventBrokerComponent.Publish(this, new InputEvents.SetInputState(true));
    }

    public void Initialize()
    {
        active = true;
        minigameUI.SetActive(true);
        eventBrokerComponent.Publish(this, new InputEvents.SetInputState(false));
    }

    public bool StartCondition()
    {
        return true;
    }

    private void Update()
    {
        if (!active) return;
        if (requiredItems.CheckInInventory(this))
        {
            Finish();
        }
    }
}
