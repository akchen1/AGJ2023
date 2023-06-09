using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Brooms : MonoBehaviour
{
    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();
    private bool onlyOnce = true;
    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.name == "Oil" && onlyOnce)
        {
			eventBrokerComponent.Publish(this, new AudioEvents.PlaySFX(Constants.Audio.SFX.BroomCrash));
            onlyOnce = false;
            eventBrokerComponent.Publish(this, new DistractionEvent.Finished());
        }
    }
}
