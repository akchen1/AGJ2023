using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brooms : MonoBehaviour
{
    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();
    public bool finished = false;
    private void OnTriggerEnter2D(Collider2D other)
    {
        if (other.gameObject.name == "Oil")
        {
            eventBrokerComponent.Publish(this, new DistractionEvent.Finished());
        }
    }

}
