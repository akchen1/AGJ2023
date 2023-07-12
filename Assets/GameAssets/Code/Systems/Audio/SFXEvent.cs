using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SFXEvent : MonoBehaviour
{
    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

    public void PlaySFX(string SFXName)
    {
        eventBrokerComponent.Publish(this, new AudioEvents.PlaySFX(SFXName));
    }
}
