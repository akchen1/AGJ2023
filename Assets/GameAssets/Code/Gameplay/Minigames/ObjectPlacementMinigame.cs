using UnityEngine;

public class ObjectPlacementMinigame : IMinigame
{
    // Need some kind of indicator maybe for desired position. Either ghostly silluette of object or arrow indicator

    private GameObject placementObject; // Object we want to move
    private Collider2D desiredBounds;   // Where we want the object to be
    private bool active;    // Internal use, to make sure Finishminigame is called only once

    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

    public ObjectPlacementMinigame(GameObject placementObject, Collider2D desiredPosition)
    {
        this.placementObject = placementObject;
        this.desiredBounds = desiredPosition;
    }

    public void Finish()
    {
        // Cleanup and result
        Debug.Log("Object placement finished");
    }

    public void Initialize()
    {
        Debug.Log("Initializing placement minigame");
        active = true;
    }

    public void PhysicsUpdate()
    {
    }

    public void Update()
    {
        if (!active) return;

        if (desiredBounds.OverlapPoint(placementObject.transform.position))
        {
            eventBrokerComponent.Publish(this, new MinigameEvents.EndMinigame());
            active = false;
        }
    }
}
