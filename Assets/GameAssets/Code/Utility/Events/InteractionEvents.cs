using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class InteractionEvents
{
    public class Interact
    {
        public readonly IInteractable Interactable;
        public Action<bool> Response;

        public Interact(IInteractable interactable, Action<bool> response)
        {
            Interactable = interactable;
            Response = response;
        }
    }

    public class InteractEnd
    {

    }

    public class CancelPendingInteraction
    {

    }
}
