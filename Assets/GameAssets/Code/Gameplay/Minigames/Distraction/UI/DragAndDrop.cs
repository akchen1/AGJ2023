using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class DragAndDrop : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public static bool isDragging = false;
    private Transform originalParent;
    private Vector2 originalPosition;
    private Canvas canvas;
    private RectTransform rectTransform;
    private CanvasGroup canvasGroup;

    private void Awake()
    {
        canvas = GetComponentInParent<Canvas>();
        rectTransform = GetComponent<RectTransform>();
        canvasGroup = GetComponent<CanvasGroup>();
    }
    private void Start() {
        
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        Debug.Log("Hello");
        transform.GetComponent<Rigidbody2D>().isKinematic = true;
        transform.GetComponent<Collider2D>().enabled = false;
        isDragging = true;
        originalParent = transform.parent;
        originalPosition = rectTransform.anchoredPosition;
        transform.SetParent(canvas.transform);
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        canvasGroup.blocksRaycasts = true;
        originalParent.GetComponent<Holder>().isHovering = false;
        GameObject otherHolder = eventData.pointerCurrentRaycast.gameObject;
        if (otherHolder != null && otherHolder.GetComponent<Holder>() && otherHolder != originalParent.gameObject)
        {

            RectTransform otherChildRect = otherHolder.transform.GetChild(0).GetComponent<RectTransform>();
            Vector2 otherChildOriginalPosition = otherChildRect.anchoredPosition;
            Vector2 otherParentOriginalPosition = otherHolder.GetComponent<RectTransform>().anchoredPosition;
            Vector2 originalParentPosition = originalParent.GetComponent<RectTransform>().anchoredPosition;

            otherChildRect.SetParent(originalParent);
            otherChildRect.anchoredPosition = new Vector2(originalPosition.x,otherChildOriginalPosition.y);
            otherChildRect.parent.GetComponent<RectTransform>().anchoredPosition = new Vector2 (originalParentPosition.x, otherParentOriginalPosition.y);

            RectTransform originalParentRect = originalParent.GetComponent<RectTransform>();
            transform.SetParent(otherHolder.transform);
            transform.parent.GetComponent<RectTransform>().anchoredPosition = new Vector2 (otherParentOriginalPosition.x, originalParentPosition.y);
            rectTransform.anchoredPosition = new Vector2(otherChildOriginalPosition.x,originalPosition.y); // Center within the holder


        }
        else
        {
            transform.SetParent(originalParent);
            rectTransform.anchoredPosition = originalPosition;
        }
        transform.GetComponent<Rigidbody2D>().isKinematic = false;
        transform.GetComponent<Collider2D>().enabled = true;
    }
}