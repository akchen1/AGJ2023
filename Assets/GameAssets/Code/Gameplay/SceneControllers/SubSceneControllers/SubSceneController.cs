using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SubSceneController
{
	[SerializeField] protected CinemachineVirtualCamera subsceneCamera;
	[SerializeField] protected Transform subsceneTeleportMarker;

	private bool isActive = false;

    protected EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

	public virtual void Enable()
	{
		subsceneCamera.enabled = true;
        eventBrokerComponent.Publish(this, new PlayerEvents.SetPlayerPosition(subsceneTeleportMarker.position));
		isActive = true;
    }

    public virtual void Disable() 
	{
		subsceneCamera.enabled = false;
		isActive = false;
	}

}
