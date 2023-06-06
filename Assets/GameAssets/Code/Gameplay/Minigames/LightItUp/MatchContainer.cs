using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class MatchContainer : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    [SerializeField] private GameObject matchPrefab;
    [SerializeField] private Transform matchParent;

    private void OnEnable()
    {
        
    }

    private void OnDisable()
    {
        
    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        GameObject match = Instantiate(matchPrefab, matchParent);
        eventData.pointerDrag = match;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Required here to work
    }
}
