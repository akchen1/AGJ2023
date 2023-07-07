using DS.ScriptableObjects;
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

    [SerializeField] private DSDialogueSO invalidSceneDialogue;

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
            invalidSceneDialogue.Interact(this, Constants.Interaction.InteractionType.Virtual);
            return false;
        }
        return requiredItems.CheckInInventory(this);
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
        endMinigameItems.AddToInventory(this);
        requiredItems.RemoveFromInventory(this);
    }

    private IEnumerator DelayedFinish()
    {
        yield return new WaitForSeconds(2);
        Finish();
    }
}
