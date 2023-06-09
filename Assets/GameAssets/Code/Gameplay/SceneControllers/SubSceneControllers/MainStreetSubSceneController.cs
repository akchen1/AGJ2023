using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class MainStreetSubSceneController : SubSceneController
{
    public override void Enable()
    {
        base.Enable();
    }

    public override void Disable()
    {
        base.Disable();
        eventBrokerComponent.Publish(this, new PlayerEvents.GetPlayerPosition(position =>
        {
            subsceneTeleportMarker.position = position;
        }));
    }
}
