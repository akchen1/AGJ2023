using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScrollManual : MonoBehaviour, IMinigame
{
    [SerializeField] private GameObject scrollUI;

    [SerializeField] private InventoryItem candle;
    [SerializeField] private InventoryItem vial;
    [SerializeField] private InventoryItem weath;
    [SerializeField] private InventoryItem gem;
    [SerializeField] private InventoryItem clay;

    [SerializeField] private GameObject candleIcon;
    [SerializeField] private GameObject vialIcon;
    [SerializeField] private GameObject weathIcon;
    [SerializeField] private GameObject gemIcon;
    [SerializeField] private GameObject clayIcon;

    [SerializeField] private Image sideB;
    [SerializeField] private Sprite backSideCondition1;
    [SerializeField] private Sprite backSideCondition2;
    [SerializeField] private Image closeButton;

    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

    public void Finish()
    {
        scrollUI.SetActive(false);
        eventBrokerComponent.Publish(this, new MinigameEvents.EndMinigame());
    }

    public void Initialize()
    {
        bool hasCandle = HasItem(candle);
        bool hasVial = HasItem(vial);
        bool hasWreath = HasItem(weath);
        bool hasGem = HasItem(gem);
        bool hasClay = HasItem(clay);

        candleIcon.SetActive(hasCandle);
        vialIcon.SetActive(hasVial);
        weathIcon.SetActive(hasWreath);
        gemIcon.SetActive(hasGem);
        clayIcon.SetActive(hasClay);

        if (hasCandle && hasVial && hasWreath && hasGem && hasClay)
        {
            sideB.sprite = backSideCondition1;
        }

        scrollUI.SetActive(true);
    }

    public bool StartCondition()
    {
        return true;
    }

    private bool HasItem(InventoryItem item)
    {
        bool hasItem = false;
        eventBrokerComponent.Publish(this, new InventoryEvents.HasItem(callback =>
        {
            hasItem = callback;
        }, item));
        return hasItem;
    }
}