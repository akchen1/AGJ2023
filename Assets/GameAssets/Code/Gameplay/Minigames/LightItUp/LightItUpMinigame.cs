using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.SceneManagement;

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
        this.EndMinigame();
    }

    public void Initialize()
    {
        active = true;
        LightItUpUI.SetActive(true);
    }

    public bool StartCondition()
    {
        Debug.Log(SceneManager.GetActiveScene().name);
        if (SceneManager.GetActiveScene().name != Constants.SceneNames.ClearingScene9)
        {
            // TODO: Start dialogue
            return false;
        }
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
        eventBrokerComponent.Publish(this, new InventoryEvents.AddItem(endMinigameItems.ToArray()));
        eventBrokerComponent.Publish(this, new InventoryEvents.RemoveItem(requiredItems.ToArray()));
    }

    private IEnumerator DelayedFinish()
    {
        yield return new WaitForSeconds(2);
        Finish();
    }
}
