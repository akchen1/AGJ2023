using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public bool HasInteractionDistance { get; set; }
    public FloatReference InteractionDistance { get; set; }
    public void Interact();

}
