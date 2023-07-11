using Cinemachine;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SubSceneController
{
	public virtual Constants.Scene7SubScenes Subscene { get; }

	[SerializeField] protected CinemachineVirtualCamera subsceneCamera;
	[SerializeField] protected Transform subsceneTeleportMarker;
	protected virtual string subSceneMusic { get; }

	protected bool isActive = false;

    protected EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

	public virtual void Enable(bool teleportPlayer = true)
	{
		subsceneCamera.enabled = true;
		if (teleportPlayer)
			eventBrokerComponent.Publish(this, new PlayerEvents.SetPlayerPosition(subsceneTeleportMarker.position));
		eventBrokerComponent.Publish(this, new AudioEvents.PlayMusic(subSceneMusic, true));
		isActive = true;
    }

    public virtual void Disable() 
	{
		subsceneCamera.enabled = false;
		isActive = false;
	}

	public virtual void Update()
	{

	}
}
