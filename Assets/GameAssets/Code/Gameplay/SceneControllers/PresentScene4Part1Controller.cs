using UnityEngine;

public class PresentScene4Part1Controller : MonoBehaviour
{
    [SerializeField] private DialogueInteraction badu;
    [SerializeField] private RuntimeAnimatorController baduMothFly;
    [SerializeField] private GameObject bedroomDoor;

    private bool isTalkingWithBadu = false;

    EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

	private void Start()
	{
		eventBrokerComponent.Publish(this, new AudioEvents.PlayMusic(Constants.Audio.Music.LivingRoom, true));
	}

	private void OnEnable()
    {
        eventBrokerComponent.Subscribe<InteractionEvents.Interact>(InteractHandler);
        eventBrokerComponent.Subscribe<InteractionEvents.InteractEnd>(InteractEndHandler);
    }

    private void OnDisable()
    {
        eventBrokerComponent.Unsubscribe<InteractionEvents.Interact>(InteractHandler);
        eventBrokerComponent.Unsubscribe<InteractionEvents.InteractEnd>(InteractEndHandler);
    }

    private void InteractHandler(BrokerEvent<InteractionEvents.Interact> inEvent)
    {
        if (!(inEvent.Sender is UnityEngine.Object)) return;
        UnityEngine.Object sender = (UnityEngine.Object)inEvent.Sender;
        Debug.Log(sender);
        Debug.Log(badu);
        if (sender == badu)
        {
            bedroomDoor.GetComponent<DialogueInteraction>().enabled = false;
            bedroomDoor.GetComponent<SceneChangeInteraction>().enabled = true;
            isTalkingWithBadu = true;
        }
    }

    private void InteractEndHandler(BrokerEvent<InteractionEvents.InteractEnd> inEvent)
    {
        if (!isTalkingWithBadu) return;
        badu.GetComponent<Animator>().runtimeAnimatorController = baduMothFly;
        isTalkingWithBadu = false;
    }
}
