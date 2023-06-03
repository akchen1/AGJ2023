using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;
public class VasePieceUI : MonoBehaviour, IBeginDragHandler, IDragHandler, IEndDragHandler
{
    public RectTransform RectTransform { get; private set; }
    [field:SerializeField] public RectTransform TargetTransform { get; private set; }
    public bool Grabbing { get; private set; }

    private void Awake()
    {
        RectTransform = GetComponent<RectTransform>();
        Grabbing = false;
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
    }
}
