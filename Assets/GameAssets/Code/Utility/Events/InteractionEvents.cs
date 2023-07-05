using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionEvents
{
    public class Interact
    {
        public readonly UnityEngine.Object Interactable;
        public Action<bool> Response;
        public readonly Constants.Interaction.InteractionType InteractType;

        public Interact(UnityEngine.Object interactable, Action<bool> response, Constants.Interaction.InteractionType interactType = Constants.Interaction.InteractionType.World)
        {
            Interactable = interactable;
            Response = response;
            InteractType = interactType;
        }
    }

    public class InteractEnd
    {

    }

    public class CancelPendingInteraction
    {

    }
}
