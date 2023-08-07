using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class SeeBooks : MonoBehaviour, IPointerClickHandler, IInteractableWorld
{
    [SerializeField] GameObject panel;
    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

    [SerializeField] private List<Image> books;
    private int currentOpenBookIndex = -1;

    [field: SerializeField] public bool HasInteractionDistance { get; set; } = false;
    [field: SerializeField] public FloatReference InteractionDistance { get; set; }

    public void CloseBooks()
    {
        CloseBook();
        eventBrokerComponent.Publish(this, new InteractionEvents.InteractEnd());
        panel.SetActive(false);
    }

    public void Interact()
    {
        panel.SetActive(true);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (gameObject.Interact())
        {
            Interact();
        }
    }

    public void SelectBook(int bookIndex)
    {
        if (bookIndex == currentOpenBookIndex)
        {
            CloseBook();
            return;
        }

        for (int i = 0; i < books.Count; i++)
        {
            if (i == bookIndex) continue;
            books[i].raycastTarget = false;
        }
        books[bookIndex].transform.GetChild(0).gameObject.SetActive(true);
        currentOpenBookIndex = bookIndex;
    }

    public void CloseBook()
    {
        if (currentOpenBookIndex < 0) return;
        books[currentOpenBookIndex].transform.GetChild(0).gameObject.SetActive(false);
        currentOpenBookIndex = -1;
        foreach (Image book in books)
        {
            book.raycastTarget = true;
        }
    }
}
