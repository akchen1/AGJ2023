using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMinigame
{
    public bool StartCondition();
    //// Called once when StartMinigame Event is fired
    public void Initialize();

    //// Called once when EndMinigame Event is fired
    public void Finish();
}
