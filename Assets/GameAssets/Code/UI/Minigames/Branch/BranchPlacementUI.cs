using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchPlacementUI : ObjectPlacementPieceUI
{
    [SerializeField] private RectTransform branchTransform;
    protected override bool CheckOverlapTarget()
    {
        return base.CheckOverlapTarget() && branchTransform.rotation == TargetTransform.rotation;
    }
}
