using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SanitySystem 
{
    [SerializeField] private FloatReference SanityLevel;

	private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

	public SanitySystem() 
    {
        eventBrokerComponent.Subscribe<SanityEvents.ChangeSanity>(ChangeSanityHandler);
    }

    ~SanitySystem()
    {
        eventBrokerComponent.Unsubscribe<SanityEvents.ChangeSanity>(ChangeSanityHandler);
    }

    private void ChangeSanityHandler(BrokerEvent<SanityEvents.ChangeSanity> inEvent)
    {
        throw new NotImplementedException();
    }
}
