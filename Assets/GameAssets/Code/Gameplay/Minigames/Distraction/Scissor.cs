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
    private float initalWaitTime = 3f;
    private float addTime = 2f;
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
        gameObject.GetComponent<Animator>().enabled = true;
        rectTransform.anchoredPosition += eventData.delta / canvas.scaleFactor;
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        // Enable raycasting on the UI element to restore normal interaction
        gameObject.GetComponent<Animator>().enabled = false;
        canvasGroup.blocksRaycasts = true;
    }
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "BalloonString")
        {
            float waitTime = initalWaitTime;
            GameObject[] targetObjects = GameObject.FindGameObjectsWithTag("Holder");
            foreach(GameObject go in targetObjects){
                go.GetComponent<Image>().enabled = false;
                go.transform.GetChild(0).gameObject.GetComponent<DragAndDrop>().enabled = false;
                if(go.GetComponent<RightChild>().Check())
                    waitTime += addTime;
            }
			eventBrokerComponent.Publish(this, new AudioEvents.PlaySFX(Constants.Audio.SFX.Shears));
            eventBrokerComponent.Publish(this, new DistractionTimerEvent.SetDistracitonTime(waitTime));
            eventBrokerComponent.Publish(this, new DistractionEvent.Start());
            Transform balloon = other.transform.parent;
            //Add Base Change

            //
            balloon.GetComponents<Image>()[0].enabled = true;
            balloon.GetComponent<Rigidbody2D>().isKinematic = false;
            gameObject.SetActive(false);
        }
    }


}
