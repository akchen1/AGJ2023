using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class BranchPlacementUI : ObjectPlacementPieceUI, IPointerClickHandler
{
    [SerializeField] private float rotateAngle = 60f;
    private Image image;

    protected override void Awake()
    {
        base.Awake();
        image = GetComponent<Image>();
        image.alphaHitTestMinimumThreshold = 0.5f;
        TargetTransform.GetComponent<Image>().alphaHitTestMinimumThreshold = 0.5f;
    }

    protected override void Update()
    {
        if (IsInTargetPosition)
        {
            draggable.enabled = false;
            return;
        }
        
        base.Update();
    }

    protected override bool CheckOverlapTarget()
    {
        float angleDifference = Mathf.Abs(RectTransform.rotation.eulerAngles.z - TargetTransform.rotation.eulerAngles.z);
        return base.CheckOverlapTarget() && angleDifference < 1E-4;
    }

    public void RotateBranch(Vector3 rotatePoint)
    {
        //RectTransform.Rotate(Vector3.forward, rotateAngle);
        RectTransform.RotateAround(rotatePoint, Vector3.forward, rotateAngle);
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (Grabbing || IsInTargetPosition) return;
        RotateBranch(eventData.pressPosition);
    }
}
