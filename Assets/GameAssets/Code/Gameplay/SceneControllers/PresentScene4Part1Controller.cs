using DS.ScriptableObjects;
using System;
using UnityEngine;

public class PresentScene4Part1Controller : MonoBehaviour
{

    [SerializeField] private GameObject badu;

    [SerializeField] private RuntimeAnimatorController baduMothFly;
    private Animator baduAnimator;
    [SerializeField] private GameObject bedroomDoor;

	EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

	private void Start()
	{
        baduAnimator = badu.GetComponent<Animator>();
        baduAnimator.SetBool("isBadu", true);

        eventBrokerComponent.Publish(this, new AudioEvents.PlayMusic(Constants.Audio.Music.LivingRoom, true));
	}

    public void BaduInteractionStarted()
    {
        bedroomDoor.GetComponent<DialogueInteraction>().enabled = false;
        bedroomDoor.GetComponent<SceneChangeInteraction>().enabled = true;
    }

    public void BaduInteractionEnded()
    {
        baduAnimator.SetBool("isBadu", false);
        badu.GetComponent<CapsuleCollider2D>().enabled = false;
        badu.GetComponent<CircleCollider2D>().enabled = true;
    }
}
