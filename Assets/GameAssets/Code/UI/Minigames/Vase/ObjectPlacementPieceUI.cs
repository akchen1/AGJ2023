using System.Collections;
using System.Collections.Generic;
using System.Runtime.InteropServices.WindowsRuntime;
using UnityEditor.U2D.Path;
using UnityEngine;

[RequireComponent(typeof(DraggableUI))]
public class ObjectPlacementPieceUI : MonoBehaviour
{
    [field: SerializeField] public RectTransform TargetTransform { get; private set; }
    [SerializeField] private bool snapping = true;

    private RectTransform RectTransform { get { return draggable.RectTransform; } }
    private bool Grabbing { get { return draggable.Grabbing; } }
    public bool IsInTargetPosition { get; private set; } = false;

    private DraggableUI draggable;

    private void Awake()
    {
        draggable = GetComponent<DraggableUI>();
    }

    private void Update()
    {
        IsInTargetPosition = CheckOverlapTarget();

        if (snapping)
            Snap();
    }

    protected virtual bool CheckOverlapTarget()
    {
        if (Grabbing || !TargetTransform.RectOverlapsPoint(RectTransform))
        {
            return false;
        }
        return true;
    }

    protected void Snap()
    {
        if (!IsInTargetPosition) return;
        RectTransform.position = TargetTransform.position;
    }
}
