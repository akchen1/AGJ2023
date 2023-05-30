using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

[System.Serializable]
public class SanitySystem 
{
    [SerializeField] private FloatReference SanityLevel;

	private int currentSanity;

	private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

	public SanitySystem() 
    {
        eventBrokerComponent.Subscribe<SanityEvents.ChangeSanity>(ChangeSanityHandler);
		eventBrokerComponent.Subscribe<SanityEvents.GetSanity>(GetSanityHandler);

		currentSanity = Constants.Sanity.DefaultSanityLevel;
    }

	~SanitySystem()
    {
        eventBrokerComponent.Unsubscribe<SanityEvents.ChangeSanity>(ChangeSanityHandler);
		eventBrokerComponent.Unsubscribe<SanityEvents.GetSanity>(GetSanityHandler);
    }

    private void ChangeSanityHandler(BrokerEvent<SanityEvents.ChangeSanity> inEvent)
    {
		currentSanity = Mathf.Clamp(currentSanity + inEvent.Payload.Amount, Constants.Sanity.MinimumSanityLevel, Constants.Sanity.MaximumSanityLevel);
    }

	private void GetSanityHandler(BrokerEvent<SanityEvents.GetSanity> inEvent)
	{
		inEvent.Payload.ProcessData.DynamicInvoke(currentSanity);
	}
}
