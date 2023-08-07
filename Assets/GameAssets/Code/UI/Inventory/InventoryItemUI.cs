using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using DS.ScriptableObjects;

public class InventoryItemUI : MonoBehaviour, IPointerClickHandler, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    [SerializeField] private Image itemIcon;
    private RectTransform viewPort;
    public InventoryItem inventoryItem { get; private set; }

    private bool selected;
    private Vector3 mouseOffset;
    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

    private GameObject dragIcon;
    private static Canvas canvas;

    private Vector3[] viewPortCorners = new Vector3[4];
    private void Awake()
    {
        //itemIcon = GetComponent<Image>();
        if (canvas == null )
        {
            canvas = FindInParents<Canvas>(gameObject);
        }
    }

    public void Initialize(InventoryItem inventoryItem, RectTransform viewPort)
    {
        this.inventoryItem = inventoryItem;
        itemIcon.sprite = inventoryItem.ItemIcon;
        this.viewPort = viewPort;

        viewPort.GetWorldCorners(viewPortCorners);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;

        selected = !selected;
        eventBrokerComponent.Publish(this, new InventoryEvents.SelectItem(inventoryItem));
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;
        if (canvas == null) return;

        mouseOffset = transform.position - (Vector3)eventData.position;
        dragIcon = new GameObject("icon");

        dragIcon.transform.SetParent(canvas.transform, false);
        dragIcon.transform.SetAsLastSibling();
        Image image = dragIcon.AddComponent<Image>();

        image.sprite = itemIcon.sprite;
        image.preserveAspect = itemIcon.preserveAspect;
        
        //image.SetNativeSize();

        SetDraggedPosition(eventData);

        dragIcon.transform.SetAsLastSibling();   // Draw over everything
    }

    public void OnDrag(PointerEventData data)
    {
        if (data.button != PointerEventData.InputButton.Left) return;

        SetDraggedPosition(data);
        CheckScrollBounds(data);
    }

    private void CheckScrollBounds(PointerEventData data)
    {
        if (data.position.y > viewPortCorners[2].y - Constants.Inventory.InventoryScrollPadding)   // TODO: This value might need to change depending on the screen resolution
        {
            eventBrokerComponent.Publish(this, new InventoryEvents.ScrollInventory(false));
        }
        else if (data.position.y < viewPortCorners[0].y)
        {
            eventBrokerComponent.Publish(this, new InventoryEvents.ScrollInventory(true));
        }
    }

    private void SetDraggedPosition(PointerEventData data)
    {
        Vector3 globalMousePos;
        var rt = dragIcon.GetComponent<RectTransform>();
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(transform as RectTransform, data.position, data.pressEventCamera, out globalMousePos))
        {
            rt.position = globalMousePos + mouseOffset;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        if (eventData.button != PointerEventData.InputButton.Left) return;

        Destroy(dragIcon);

        GraphicRaycaster r = canvas.GetComponent<GraphicRaycaster>();
        List<RaycastResult> results = new List<RaycastResult>();
        r.Raycast(eventData, results);

        InventoryItemUI hitItem;
        foreach (RaycastResult result in results)
        {
            hitItem = result.gameObject.GetComponent<InventoryItemUI>();
            if (hitItem != null && hitItem != this)
            {
                eventBrokerComponent.Publish(this, new InventoryEvents.DragCombineItem(inventoryItem, hitItem.inventoryItem));
                return;
            }
        }
    }

    static public T FindInParents<T>(GameObject go) where T : Component
    {
        if (go == null) return null;
        var comp = go.GetComponent<T>();

        if (comp != null)
            return comp;

        Transform t = go.transform.parent;
        while (t != null && comp == null)
        {
            comp = t.gameObject.GetComponent<T>();
            t = t.parent;
        }
        return comp;
    }
}
