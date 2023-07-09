using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.EventSystems;

public class ObjectPlacementMinigame : MonoBehaviour, IMinigame
{
    [SerializeField] private GameObject minigameUI; // UI to set active
    [SerializeField] private List<ObjectPlacementPieceUI> objectPieces; // Object pieces
    [SerializeField] private List<InventoryItem> requiredItems;    // Start condition
    [SerializeField] private List<InventoryItem> obtainedItems;    // Start condition
    [SerializeField] private GameObject finishedObject; // Object to set active after finishing the minigame
    private bool active;
    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

    #region IMinigame Methods
    public bool StartCondition()
    {
        if (requiredItems.Count == 0) return true;
        bool canStart = false;
        eventBrokerComponent.Publish(this, new InventoryEvents.HasItem(callback =>
        {
            canStart = callback;
        }, requiredItems.ToArray()));
        
        return canStart;
    }

    public void Finish()
    {
        active = false;
        minigameUI.SetActive(false);
        bool isCompleted = CheckEndCondition();
        this.EndMinigame(isCompleted);
        if (!isCompleted)
        {
            ResetPieces();
            return;
        }
        if (finishedObject != null)
            finishedObject.SetActive(true);

        HandleInventoryEvents();

    }

    private void ResetPieces()
    {
        foreach (ObjectPlacementPieceUI piece in objectPieces)
        {
            piece.ResetPiece();
        }
    }

    public void Initialize()
    {
        active = true;
        minigameUI.SetActive(true);
        eventBrokerComponent.Publish(this, new InputEvents.SetInputState(false));
    }

    #endregion

    private void Update()
    {
        if (!active) return;
        if (CheckEndCondition())
            StartCoroutine(DelayedFinish());
    }

    #region Main Methods
    protected virtual bool CheckEndCondition()
    {
        for (int i = 0; i < objectPieces.Count; i++)
        {
            ObjectPlacementPieceUI piece = objectPieces[i];
            if (!piece.IsInTargetPosition)
            {
                return false;
            }
        }
        return true;
    }
    #endregion

    #region Utility Methods
    protected virtual IEnumerator DelayedFinish()
    {
        active = false;
        yield return new WaitForSeconds(2f);
        Finish();
    }

    private void HandleInventoryEvents()
    {
        if (obtainedItems.Count != 0)
            eventBrokerComponent.Publish(this, new InventoryEvents.AddItem(obtainedItems.ToArray()));
        if (requiredItems.Count != 0)
            eventBrokerComponent.Publish(this, new InventoryEvents.RemoveItem(requiredItems.ToArray()));
    }
    #endregion
}
