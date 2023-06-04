using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameTest : MonoBehaviour
{
    [SerializeField] private ObjectPlacementMinigameTest minigame;

    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();
    void Start()
    {
        eventBrokerComponent.Publish(this, new MinigameEvents.StartMinigame(minigame));
    }

    // Update is called once per frame
    void Update()
    {
        
    }
}
