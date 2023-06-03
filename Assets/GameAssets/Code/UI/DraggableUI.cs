using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class DraggableUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public RectTransform RectTransform { get; private set; }
    public bool Grabbing { get; private set; }

    [SerializeField] private bool keepInBounds;
    [SerializeField] private Collider2D bounds;
    private Vector3 lastPositionInBounds;

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
        SetDraggedPosition(eventData);
        Grabbing = true;
        transform.SetAsLastSibling();
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
            RectTransform.position = globalMousePos;
        }
    }

    public void OnEndDrag(PointerEventData eventData)
    {
        Grabbing = false;
        if (keepInBounds) 
        { 
            transform.position = lastPositionInBounds;
        }
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
