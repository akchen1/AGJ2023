using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class VaseMinigame : MonoBehaviour, IMinigame
{
    [SerializeField] private GameObject vaseUI;
    [SerializeField] private List<VasePieceUI> vasePieces;
    [SerializeField] private List<RectTransform> vaseTargets;
    [SerializeField] private bool snapping;
    [SerializeField] private InventoryItem brokenVasePieces;
    private bool active;
    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

    #region IMinigame Methods
    public bool StartCondition()
    {
        bool canStart = false;
        eventBrokerComponent.Publish(this, new InventoryEvents.HasItem(brokenVasePieces, callback =>
        {
            canStart = callback;
        }));
        
        return canStart;
    }

    public void Finish()
    {
        active = false;
        vaseUI.SetActive(false);
        eventBrokerComponent.Publish(this, new MinigameEvents.EndMinigame());
        eventBrokerComponent.Publish(this, new InventoryEvents.RemoveItem(brokenVasePieces));
        eventBrokerComponent.Publish(this, new InputEvents.SetInputState(true));
    }

    public void Initialize()
    {
        active = true;
        vaseUI.SetActive(true);
        eventBrokerComponent.Publish(this, new InputEvents.SetInputState(false));
    }

    #endregion

    private void Update()
    {
        if (!active) return;
        if (CheckEndCondition())
            Finish();
    }

    #region Main Methods
    private bool CheckEndCondition()
    {
        for (int i = 0; i < vasePieces.Count; i++)
        {
            if (vasePieces[i].Grabbing) return false;

            RectTransform target = vaseTargets[i];
            if (!RectOverlapsPoint(target, vasePieces[i].RectTransform))
            {
                return false;
            }

            if (snapping)
                vasePieces[i].RectTransform.position = target.position;
        }
        return true;
    }
    #endregion

    #region Utility Methods
    private bool RectOverlapsPoint(RectTransform rectTrans1, RectTransform rectTrans2)
    {
        Rect rect1 = new Rect(rectTrans1.localPosition.x, rectTrans1.localPosition.y, rectTrans1.rect.width, rectTrans1.rect.height);
        Rect rect2 = new Rect(rectTrans2.localPosition.x, rectTrans2.localPosition.y, rectTrans2.rect.width/2, rectTrans2.rect.height/2);
        return rect1.Overlaps(rect2);
    }
    #endregion
}
