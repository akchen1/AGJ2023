using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[RequireComponent(typeof(DraggableUI))]
public class ObjectPlacementPieceUI : MonoBehaviour
{
    [field: SerializeField] public RectTransform TargetTransform { get; private set; }

    public RectTransform RectTransform { get { return draggable.RectTransform; } }
    public bool Grabbing { get { return draggable.Grabbing; } }

    private DraggableUI draggable;

    private void Awake()
    {
        draggable = GetComponent<DraggableUI>();
    }
}
