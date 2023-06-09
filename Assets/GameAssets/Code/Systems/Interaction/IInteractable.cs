using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IInteractable
{
    public FloatReference InteractionDistance { get; set; }
    public void Interact();

}
