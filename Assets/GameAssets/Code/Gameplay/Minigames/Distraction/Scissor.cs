using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using UnityEngine.EventSystems;

public class Scissor :  MonoBehaviour, IDragHandler, IBeginDragHandler, IEndDragHandler
{
    private RectTransform rectTransform;
    private Canvas canvas;
    private CanvasGroup canvasGroup;
    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();
    private void Awake()
    {
        rectTransform = GetComponent<RectTransform>();
        canvas = GetComponentInParent<Canvas>();
        canvasGroup = GetComponent<CanvasGroup>();
    }

    public void OnBeginDrag(PointerEventData eventData)
    {
        // Set the UI element as the topmost element in the canvas
        rectTransform.SetAsLastSibling();
        // Disable raycasting on the UI element to allow interaction with objects behind it
        canvasGroup.blocksRaycasts = false;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Move the UI element according to the pointer position
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Enable raycasting on the UI element to restore normal interaction
        canvasGroup.blocksRaycasts = true;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "BalloonString")
        {
            GameObject[] targetObjects = GameObject.FindGameObjectsWithTag("Holder");
            foreach(GameObject go in targetObjects){
                go.GetComponent<Image>().enabled = false;
            }
            other.transform.parent.GetComponent<Rigidbody2D>().isKinematic = false;
            eventBrokerComponent.Publish(this, new DistractionEvent.Start());
        }
    }

}
