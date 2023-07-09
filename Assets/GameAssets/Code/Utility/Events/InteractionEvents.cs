using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionEvents
{
    public class Interact
    {
        public readonly object Interactable;
        public Action<bool> Response;
        public readonly Constants.Interaction.InteractionType InteractType;

        public Interact(object interactable, Action<bool> response, Constants.Interaction.InteractionType interactType = Constants.Interaction.InteractionType.World)
        {
            Interactable = interactable;
            Response = response;
            InteractType = interactType;
        }
    }

    public class InteractionStarted
    {
        public readonly object Interactable;

        public InteractionStarted(object interactable)
        {
            Interactable = interactable;
        }
    }

    public class InteractEnd
    {

    }

    public class CancelPendingInteraction
    {

    }
}
