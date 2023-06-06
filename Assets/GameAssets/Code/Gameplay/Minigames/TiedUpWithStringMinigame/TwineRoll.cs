using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class TwineRoll : MonoBehaviour, IBeginDragHandler, IDragHandler
{
    [SerializeField] private TwinePlacementPieceUI twinePrefab;
    [SerializeField] private Transform twineParent;
    [SerializeField] private List<RectTransform> possibleTargets;

    public List<TwinePlacementPieceUI> CreatedTwine { get; private set; }
    [field:SerializeField] public int CreatedTwineLimit { get; private set; } = 5;

    private void Awake()
    {
        CreatedTwine = new List<TwinePlacementPieceUI>();
    }

    private void OnEnable()
    {

    }

    private void OnDisable()
    {

    }
    public void OnBeginDrag(PointerEventData eventData)
    {
        if (CreatedTwine.Count >= CreatedTwineLimit) return;
        TwinePlacementPieceUI twine = Instantiate(twinePrefab, twineParent);
        CreatedTwine.Add(twine);
        twine.Initialize(ref possibleTargets);
        eventData.pointerDrag = twine.gameObject;
    }

    public void OnDrag(PointerEventData eventData)
    {
        // Required here to work
    }
}
