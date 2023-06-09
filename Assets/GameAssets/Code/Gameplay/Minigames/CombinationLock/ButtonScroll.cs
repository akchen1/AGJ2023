using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ButtonScroll : MonoBehaviour
{
    private ScrollRect scrollRect;
    private bool isScrolling = false;
    private Vector2 targetScrollPosition;
    private int direction = 0;
    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();
    [SerializeField] private LockCombination lockCombination;
    [SerializeField] private float scrollSpeed = 200f;
    [SerializeField] private float scrollDistance = 50f;
    [SerializeField] private int ID;

    void Start()
    {
        // Set the initial scroll position
        scrollRect = GetComponent<ScrollRect>();
        targetScrollPosition = scrollRect.content.anchoredPosition;
    }

    void Update()
    {
        if (isScrolling)
        {
            // Smoothly scroll towards the target position
            scrollRect.content.anchoredPosition = Vector2.MoveTowards(scrollRect.content.anchoredPosition, targetScrollPosition, scrollSpeed * Time.deltaTime);

            // Check if we have reached the target position
            if (Vector2.Distance(scrollRect.content.anchoredPosition, targetScrollPosition) < 0.01f)
            {
                if(direction == 1)
                {
                    RectTransform firstChild = transform.GetChild(9).GetComponent<RectTransform>();

                    // Move the first child to the bottom of the hierarchy
                    firstChild.SetAsFirstSibling();

                    // Adjust the content position to reflect the change
                    Vector2 contentPosition = scrollRect.content.anchoredPosition;
                    contentPosition.y += firstChild.rect.height;
                    scrollRect.content.anchoredPosition = contentPosition;
                }
                else if(direction == -1)
                {
                    RectTransform firstChild = transform.GetChild(0).GetComponent<RectTransform>();

                    // Move the first child to the bottom of the hierarchy
                    firstChild.SetAsLastSibling();

                    // Adjust the content position to reflect the change
                    Vector2 contentPosition = scrollRect.content.anchoredPosition;
                    contentPosition.y -= firstChild.rect.height;
                    scrollRect.content.anchoredPosition = contentPosition;
                }
                lockCombination.CheckCorrect();
                isScrolling = false;
            }
        }
    }

    public void Increment()
    {
        if (!isScrolling)
        {
            // Calculate the new target position for scrolling down
            Vector2 currentScrollPosition = scrollRect.content.anchoredPosition;
            targetScrollPosition = currentScrollPosition + new Vector2(0f, -scrollDistance);
            direction = 1;
            eventBrokerComponent.Publish(this, new AudioEvents.PlaySFX(Constants.Audio.SFX.DialTurn));

            // Start scrolling
            isScrolling = true;
        }
    }

    public void Decrement()
    {
        if (!isScrolling)
        {
            // Calculate the new target position for scrolling up
            Vector2 currentScrollPosition = scrollRect.content.anchoredPosition;
            targetScrollPosition = currentScrollPosition + new Vector2(0f, scrollDistance);
            direction = -1;
            eventBrokerComponent.Publish(this, new AudioEvents.PlaySFX(Constants.Audio.SFX.DialTurn));

            // Start scrolling
            isScrolling = true;
        }
    }


    private void OnEnable() {
        eventBrokerComponent.Subscribe<LockEvent.GetCombination>(GetCombinationHandler);
    }
    private void GetCombinationHandler(BrokerEvent<LockEvent.GetCombination> inEvent)
    {
        inEvent.Payload.ProcessData.DynamicInvoke(transform.GetChild(8).name, ID);
    }
    private void OnDisable()
    {
        eventBrokerComponent.Unsubscribe<LockEvent.GetCombination>(GetCombinationHandler);
    }
}
