using DS.ScriptableObjects;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class RecordPlayerInteraction : MonoBehaviour, IInteractableWorld, IPointerClickHandler
{

    [SerializeField] private DSDialogueSO recordPlayerInteractDialogue;

    private Animator recordPlayerAnimator;
    private bool recordPlayerPlaying = false;
    EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

    [field: SerializeField] public bool HasInteractionDistance { get; set; }
    [field: SerializeField] public FloatReference InteractionDistance { get; set; }

    private void Start()
    {
        recordPlayerAnimator = GetComponent<Animator>();
    }

    public void TurnOff()
    {
        if (recordPlayerPlaying)
        {
            ToggleRecordPlayer();
        }
    }

    public void ToggleRecordPlayer()
    {
        if (!recordPlayerPlaying)
        {
            eventBrokerComponent.Publish(this, new DialogueEvents.StartDialogue(recordPlayerInteractDialogue));
            eventBrokerComponent.Publish(this, new AudioEvents.PlayTemporaryMusic(Constants.Audio.Music.RecordPlayer));
        }
        else
        {
            eventBrokerComponent.Publish(this, new AudioEvents.StopTemporaryMusic());
        }
        recordPlayerPlaying = !recordPlayerPlaying;
        recordPlayerAnimator.SetTrigger("Toggle");
    }

    public void Interact()
    {
        if (gameObject.Interact())
        {
            ToggleRecordPlayer();
            gameObject.EndInteract();
        }
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        Interact();
    }
}
