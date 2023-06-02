using UnityEngine;

[System.Serializable]
public class ObjectPlacementMinigame : MonoBehaviour, IMinigame
{
    // Need some kind of indicator maybe for desired position. Either ghostly silluette of object or arrow indicator
    [SerializeField] private GameObject placementObject; // Object we want to move
    [SerializeField] private Collider2D desiredBounds;   // Where we want the object to be
    private bool active;    // Internal use, to make sure Finishminigame is called only once
    
    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

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

    public bool StartCondition()
    {
        return true;
    }

    public void Update()
    {
        if (!active) return;
        Debug.Log("running");
        if (desiredBounds.OverlapPoint(placementObject.transform.position))
        {
            eventBrokerComponent.Publish(this, new MinigameEvents.EndMinigame());
            active = false;
        }
    }
}
