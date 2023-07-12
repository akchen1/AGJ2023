using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BranchBundle : MonoBehaviour
{
    [SerializeField] private RectTransform branch;
    [SerializeField] private float rotateAngle = 60f;

    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();
    public void RotateBranch()
    {
        branch.Rotate(Vector3.forward, rotateAngle);
        eventBrokerComponent.Publish(this, new AudioEvents.PlaySFX(Constants.Audio.SFX.PieceRotate));
    }
}
