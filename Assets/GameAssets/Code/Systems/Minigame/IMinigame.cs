using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public interface IMinigame
{
    // Called once when StartMinigame Event is fired
    public void Initialize();

    // Called every update method
    public void Update();

    // Called every fixed update
    public void PhysicsUpdate();

    // Called once when EndMinigame Event is fired
    public void Finish();
}
