using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class DraggableUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public RectTransform RectTransform { get; private set; }
    public bool Grabbing { get; set; }

    [SerializeField] private bool keepInBounds;
    [SerializeField] private Collider2D bounds;
    private Vector3 lastPositionInBounds;

    private Vector3 mouseOffset;
    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

    private void Awake()
    {
        RectTransform = GetComponent<RectTransform>();
        Grabbing = false;

        if (keepInBounds && bounds == null)
        {
            Debug.LogError("Keep in bounds is checked but no bounds supplied");
        }
    }

    public void OnBeginDrag(PointerEventData eventData)
    {

        mouseOffset = transform.position - (Vector3)eventData.position;
        SetDraggedPosition(eventData);
        Grabbing = true;
        transform.SetAsLastSibling();
        eventBrokerComponent.Publish(this, new AudioEvents.PlaySFX(Constants.Audio.SFX.PieceLift));
    }

    public void OnDrag(PointerEventData data)
    {
        SetDraggedPosition(data);
        if (keepInBounds)
        {
            CheckInBounds();
        }
    }

    private void SetDraggedPosition(PointerEventData data)
    {
        Vector3 globalMousePos;
        if (RectTransformUtility.ScreenPointToWorldPointInRectangle(RectTransform, data.position, data.pressEventCamera, out globalMousePos))
        {
            RectTransform.position = globalMousePos + mouseOffset;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Grabbing = false;
        if (keepInBounds) 
        { 
            transform.position = lastPositionInBounds;
        }
        eventBrokerComponent.Publish(this, new AudioEvents.PlaySFX(Constants.Audio.SFX.PiecePlace));
    }

    private void CheckInBounds()
    {
        if (bounds.bounds.Contains(transform.position))
        {
            lastPositionInBounds = transform.position;
            return;
        }
    }
}
