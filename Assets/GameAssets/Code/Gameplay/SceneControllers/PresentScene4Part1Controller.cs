using DS.ScriptableObjects;
using System;
using UnityEngine;

public class PresentScene4Part1Controller : MonoBehaviour
{

    [SerializeField] private GameObject badu;
    [SerializeField] private GameObject recordPlayer;

    [SerializeField] private RuntimeAnimatorController baduMothFly;
    [SerializeField] private GameObject bedroomDoor;
    private Animator recordPlayerAnimator;

	private bool recordPlayerPlaying = false;

	EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

	private void Start()
	{
		eventBrokerComponent.Publish(this, new AudioEvents.PlayMusic(Constants.Audio.Music.LivingRoom, true));
        recordPlayerAnimator = recordPlayer.GetComponent<Animator>();
	}

    public void BaduInteractionStarted()
    {
        bedroomDoor.GetComponent<DialogueInteraction>().enabled = false;
        bedroomDoor.GetComponent<SceneChangeInteraction>().enabled = true;
    }

    public void BaduInteractionEnded()
    {
        badu.GetComponent<Animator>().runtimeAnimatorController = baduMothFly;
        badu.GetComponent<CapsuleCollider2D>().enabled = false;
        badu.GetComponent<CircleCollider2D>().enabled = true;
    }

	public void ToggleRecordPlayer()
	{
		if (!recordPlayerPlaying)
		{
			eventBrokerComponent.Publish(this, new AudioEvents.PlayTemporaryMusic(Constants.Audio.Music.RecordPlayer));
		}
		else
		{
			eventBrokerComponent.Publish(this, new AudioEvents.StopTemporaryMusic());
		}
        recordPlayerPlaying = !recordPlayerPlaying;
        recordPlayerAnimator.SetTrigger("Toggle");
        recordPlayer.GetComponent<DialogueInteraction>().enabled = !recordPlayerPlaying;
        recordPlayer.GetComponent<EmptyInteraction>().enabled = recordPlayerPlaying;
	}
}
