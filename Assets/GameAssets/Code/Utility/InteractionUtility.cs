using DS.ScriptableObjects;
using UnityEngine;
using UnityEngine.UIElements;

public static class InteractionUtility
{
    private static EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();
    public static bool Interact(this DSDialogueSO dialogue, object sender, Constants.Interaction.InteractionType interactionType = Constants.Interaction.InteractionType.World)
    {
        bool status = false;
        eventBrokerComponent.Publish(sender, new InteractionEvents.Interact(sender, (valid) =>
        {
            if (valid)
            {
                eventBrokerComponent.Publish(sender, new DialogueEvents.StartDialogue(dialogue));
                status = true;
            }
        }, interactionType));
        return status;
    }
    public static bool Interact(this IMinigame minigame, object sender, Constants.Interaction.InteractionType interactionType = Constants.Interaction.InteractionType.World)
    {
        // Check if conditions are met
        if (!minigame.StartCondition()) return false;

        // Check if there's another interaction event happening
        bool status = false;
        eventBrokerComponent.Publish(sender, new InteractionEvents.Interact(sender, (valid) =>
        {
            if (valid)
            {
                eventBrokerComponent.Publish(sender, new MinigameEvents.StartMinigame(minigame));
                status = true;
            }
        }, interactionType));
        return status;
    }

    public static bool Interact(this Object interactionObject)
    {
        Constants.Interaction.InteractionType interactionType = Constants.Interaction.InteractionType.Virtual;
        if (interactionObject is IInteractableWorld)
        {
            interactionType = Constants.Interaction.InteractionType.World;
        }
        bool status = false;
        eventBrokerComponent.Publish(interactionObject, new InteractionEvents.Interact(interactionObject, (valid) =>
        {
            status = valid;
        }, interactionType));
        return status;
    }

    public static void EndInteract(this Object interactionObject)
    {
        eventBrokerComponent.Publish(interactionObject, new InteractionEvents.InteractEnd());
    }
}
