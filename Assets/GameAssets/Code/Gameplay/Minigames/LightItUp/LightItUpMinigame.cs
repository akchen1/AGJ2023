using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class LightItUpMinigame : MonoBehaviour, IMinigame
{
    [SerializeField] private List<InventoryItem> requiredItems;
    [SerializeField] private GameObject LightItUpUI;
    [SerializeField] private Candle candle;
    private bool active = false;
    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

    public void Finish()
    {
        active = false;
        LightItUpUI.SetActive(false);
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

    private IEnumerator DelayedFinish()
    {
        yield return new WaitForSeconds(1);
        Finish();
    }
}
