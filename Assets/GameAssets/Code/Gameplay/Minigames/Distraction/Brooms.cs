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
            onlyOnce = false;
            StartCoroutine(WaitUntilFall());
        }
    }
    private IEnumerator WaitUntilFall()
    {
        while (transform.eulerAngles.z > 310f)
        {
            yield return null;
        }
        eventBrokerComponent.Publish(this, new DistractionEvent.Finished());
    }
}
