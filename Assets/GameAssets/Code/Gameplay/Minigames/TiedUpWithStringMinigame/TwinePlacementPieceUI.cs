using System.Collections;
using System.Collections.Generic;
using UnityEditor.U2D.Path;
using UnityEngine;
using UnityEngine.UI;

public class TwinePlacementPieceUI : ObjectPlacementPieceUI
{
    private List<RectTransform> possibleTargets;
    [SerializeField] private Sprite tiedTwineSprite;
    private Image image;

    protected override void Awake()
    {
        base.Awake();
        image = GetComponent<Image>();
    }

    protected override void Update()
    {
        if (possibleTargets == null) return;
        if (Grabbing) return;
        if (IsInTargetPosition) return;

        base.Update();
    }

    public void Initialize(ref List<RectTransform> possibleTargets)
    {
        this.possibleTargets = possibleTargets;
        draggable.Grabbing = true;
    }

    protected override bool CheckOverlapTarget()
    {
        foreach (RectTransform target in possibleTargets)
        {
            if (target.RectOverlapsPoint(RectTransform))
            {
                TargetTransform = target;
                possibleTargets.Remove(target);
                return true;
            }
        }

        return false;
    }

    protected override void Snap()
    {
        if (!IsInTargetPosition) return;
        RectTransform.position = TargetTransform.position;
        RectTransform.rotation = TargetTransform.rotation;
        IsInTargetPosition = true;
        image.sprite = tiedTwineSprite;
        draggable.enabled = false;
    }
}
