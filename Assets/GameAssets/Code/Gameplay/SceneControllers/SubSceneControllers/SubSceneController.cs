using Cinemachine;
using DS.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SubSceneController
{
	public virtual Constants.Scene7SubScenes Subscene { get; }

	[SerializeField] protected CinemachineVirtualCamera subsceneCamera;
	[SerializeField] protected Transform subsceneTeleportMarker;

	[SerializeField] protected DSDialogueSO baduSubsceneDialogue;
	[SerializeField] protected DSDialogueSO maeveSubsceneDialogue;

	[SerializeField] private bool teleportPlayer;
	[SerializeField] private bool teleportBadu;
	[SerializeField] private bool teleportMaeve;

    protected virtual string subSceneMusic { get; }

	protected bool isActive = false;

    protected EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

	public virtual void Enable(bool overrideTeleport = false)
    {
        subsceneCamera.enabled = true;
		if (!overrideTeleport)
			TeleportCharacters();
        eventBrokerComponent.Publish(this, new AudioEvents.PlayMusic(subSceneMusic, true));
        SetSubsceneDialogue();
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
    private void TeleportCharacters()
    {
        if (teleportPlayer)
            eventBrokerComponent.Publish(this, new PlayerEvents.SetPlayerPosition(subsceneTeleportMarker.position));
        if (teleportBadu)
            eventBrokerComponent.Publish(this, new Scene7Events.SetBaduPosition(subsceneTeleportMarker.position));
        if (teleportMaeve)
            eventBrokerComponent.Publish(this, new Scene7Events.SetMaevePosition(subsceneTeleportMarker.position));
    }

	protected virtual void SetSubsceneDialogue()
	{
        if (baduSubsceneDialogue != null)
        {
			eventBrokerComponent.Publish(this, new Scene7Events.SetBaduSubsceneDialogue(baduSubsceneDialogue));
        }
		if (maeveSubsceneDialogue != null)
		{
			eventBrokerComponent.Publish(this, new Scene7Events.SetMaeveSubsceneDialogue(maeveSubsceneDialogue));
		}
    }
}
