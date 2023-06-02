using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShovelMinigame : MonoBehaviour, IMinigame
{
    private bool active;
    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();
    public void Finish()
    {
        active = false;
        eventBrokerComponent.Publish(this, new MinigameEvents.EndMinigame());
    }

    public void Initialize()
    {
        active = true;
    }

    public bool StartCondition()
    {
        return true;
    }

    private void Update()
    {
        if (!active) return;
        // TODO: Implement

        Finish();
    }
}
