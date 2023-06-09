using UnityEngine;

[RequireComponent(typeof(DraggableUI))]
public class ObjectPlacementPieceUI : MonoBehaviour
{
    [field: SerializeField] public RectTransform TargetTransform { get; protected set; }
    [SerializeField] protected bool snapping = true;

    protected RectTransform RectTransform { get { return draggable.RectTransform; } }
    protected bool Grabbing { get { return draggable.Grabbing; } }
    public bool IsInTargetPosition { get; protected set; } = false;

    protected DraggableUI draggable;

    protected virtual void Awake()
    {
        draggable = GetComponent<DraggableUI>();
    }

    protected virtual void Update()
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

    protected virtual void Snap()
    {
        if (!IsInTargetPosition) return;
        RectTransform.position = TargetTransform.position;
    }
}
