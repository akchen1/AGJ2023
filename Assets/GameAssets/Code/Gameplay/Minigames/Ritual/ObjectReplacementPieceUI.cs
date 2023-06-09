using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ObjectReplacementPieceUI : ObjectPlacementPieceUI
{
    public bool CanPlace = false;
    protected override void Update()
    {
        if (!CanPlace) return;
        base.Update();
        if (IsInTargetPosition)
        {
            foreach (Image image in TargetTransform.GetComponentsInChildren<Image>())
            {
                image.enabled = true;
            }
            gameObject.SetActive(false);
        }
    }
}
