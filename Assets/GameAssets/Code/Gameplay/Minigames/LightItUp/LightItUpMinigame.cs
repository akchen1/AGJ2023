using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightItUpMinigame : MonoBehaviour, IMinigame
{
    [SerializeField] private List<InventoryItem> requiredItems;
    [SerializeField] private List<InventoryItem> endMinigameItems; // Items to give the player upon finishing the minigame
    [SerializeField] private GameObject LightItUpUI;
    [SerializeField] private Candle candle;



    private bool active = false;
    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

    public void Finish()
    {
        active = false;
        LightItUpUI.SetActive(false);
        HandleInventoryEvents();
        eventBrokerComponent.Publish(this, new MinigameEvents.EndMinigame());
    }

    public void Initialize()
    {
        active = true;
        LightItUpUI.SetActive(true);
    }

    public bool StartCondition()
    {
        return requiredItems.CheckIfHasAllRequiredItems(this);
    }

    private void Update()
    {
        if (!active) return;
        if (candle.IsLit)
        {
            StartCoroutine(DelayedFinish());
            active = false;
        }
    }

    private void HandleInventoryEvents()
    {
        foreach (InventoryItem item in endMinigameItems)
        {
            eventBrokerComponent.Publish(this, new InventoryEvents.AddItem(item));
        }

        foreach (InventoryItem item in requiredItems)
        {
            eventBrokerComponent.Publish(this, new InventoryEvents.RemoveItem(item));
        }
    }

    private IEnumerator DelayedFinish()
    {
        yield return new WaitForSeconds(2);
        Finish();
    }
}
