using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Analytics;
using UnityEngine.EventSystems;

public class ObjectPlacementMinigame : MonoBehaviour, IMinigame
{
    [SerializeField] private GameObject minigameUI; // UI to set active
    [SerializeField] private List<ObjectPlacementPieceUI> objectPieces; // Object pieces
    [SerializeField] private bool snapping; // Snaps to correct position
    [SerializeField] private InventoryItem requiredItem;    // Start condition
    [SerializeField] private GameObject finishedObject; // Object to set active after finishing the minigame
    private bool active;
    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

    #region IMinigame Methods
    public bool StartCondition()
    {
        if (requiredItem == null) return true;
        bool canStart = false;
        eventBrokerComponent.Publish(this, new InventoryEvents.HasItem(requiredItem, callback =>
        {
            canStart = callback;
        }));
        
        return canStart;
    }

    public void Finish()
    {
        active = false;
        minigameUI.SetActive(false);
        if (finishedObject != null)
            finishedObject.SetActive(true);

        eventBrokerComponent.Publish(this, new MinigameEvents.EndMinigame());
        eventBrokerComponent.Publish(this, new InputEvents.SetInputState(true));
        if (requiredItem != null)
            eventBrokerComponent.Publish(this, new InventoryEvents.RemoveItem(requiredItem));
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
    private bool CheckEndCondition()
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
    private bool RectOverlapsPoint(RectTransform rectTrans1, RectTransform rectTrans2)
    {
        Rect rect1 = new Rect(rectTrans1.localPosition.x - rectTrans1.rect.width/2, rectTrans1.localPosition.y-rectTrans1.rect.height/2, rectTrans1.rect.width, rectTrans1.rect.height);
        Rect rect2 = new Rect(rectTrans2.localPosition.x, rectTrans2.localPosition.y, 1f, 1f);
        return rect1.Overlaps(rect2);
    }

    protected virtual IEnumerator DelayedFinish()
    {
        active = false;
        yield return new WaitForSeconds(2f);
        Finish();
    }
    #endregion
}
