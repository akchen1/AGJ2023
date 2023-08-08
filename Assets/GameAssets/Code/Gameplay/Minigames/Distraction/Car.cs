using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Car : MonoBehaviour
{
    private Rigidbody2D rbody;

    private bool isFirstPlay = true;
    private bool isStarted = false;
    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

    private void Start()
    {
        rbody = GetComponent<Rigidbody2D>();
    }

    private void OnEnable()
    {
        eventBrokerComponent.Subscribe<DistractionEvent.Start>(StartHandler);
    }

    private void OnDisable()
    {
        eventBrokerComponent.Unsubscribe<DistractionEvent.Start>(StartHandler);
    }

    private void StartHandler(BrokerEvent<DistractionEvent.Start> obj)
    {
        isStarted = true;
    }

    private void Update()
    {
        if (!isStarted) return;
        if (rbody.velocity.magnitude > 0.1f && isFirstPlay)
        {
            eventBrokerComponent.Publish(this, new AudioEvents.PlaySFX(Constants.Audio.SFX.ToyCarRoll));
            isFirstPlay = false;
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Bottle")
        {
            eventBrokerComponent.Publish(this, new AudioEvents.PlaySFX(Constants.Audio.SFX.BottleHit));
        }
    }
}
