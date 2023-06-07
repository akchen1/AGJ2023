using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    private Transform originalParent;
    public Vector2 originalPosition;
    private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;
    public static bool isDragging = false;

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
        
    }
    private void Start() {
        originalPosition = rectTransform.anchoredPosition;
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        isDragging = true;
        originalParent = transform.parent;
        transform.SetParent(canvas.transform);
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        isDragging = false;
        canvasGroup.blocksRaycasts = true;

        GameObject holder = eventData.pointerCurrentRaycast.gameObject;
        if (holder != null && holder.GetComponent<Holder>() && holder != originalParent.gameObject)
        {
            RectTransform otherChildRect = holder.transform.GetChild(0).GetComponent<RectTransform>();
            // Check if the holder already has a child
            if (holder.transform.childCount > 0)
            {
                // Swap positions with the current child
                otherChildRect.SetParent(originalParent);
                otherChildRect.anchoredPosition = originalPosition;
            }

            // Move the dropped GameObject to the new holder
            transform.SetParent(holder.transform);
            rectTransform.anchoredPosition = otherChildRect.GetComponent<DragAndDrop>().originalPosition; // Center within the holder
        }
        else
        {
            // Return the GameObject to its original position
            transform.SetParent(originalParent);
            rectTransform.anchoredPosition = originalPosition;
        }
    }
}