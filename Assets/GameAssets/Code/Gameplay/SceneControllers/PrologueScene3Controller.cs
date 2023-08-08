using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PrologueScene3Controller : MonoBehaviour
{
    EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

    void Start()
    {
        eventBrokerComponent.Publish(this, new PostProcessingEvents.SetVignette(0.1f));
    }
}
