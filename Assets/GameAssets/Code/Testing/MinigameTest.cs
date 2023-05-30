using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameTest : MonoBehaviour
{
    [SerializeField] private GameObject movementObject;
    [SerializeField] private Collider2D target;

    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();
    void Start()
    {
        eventBrokerComponent.Publish(this, new MinigameEvents.StartMinigame(new ObjectPlacementMinigame(movementObject, target)));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
