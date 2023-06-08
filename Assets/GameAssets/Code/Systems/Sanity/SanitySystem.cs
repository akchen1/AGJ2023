using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using static Constants.Sanity;

[System.Serializable]
public class SanitySystem 
{
	private int currentSanity;

	public AnimationCurve sanityCurve;

	private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

	public SanitySystem() 
    {
        eventBrokerComponent.Subscribe<SanityEvents.ChangeSanity>(ChangeSanityHandler);
		eventBrokerComponent.Subscribe<SanityEvents.GetSanity>(GetSanityHandler);

		currentSanity = DefaultSanityLevel;
    }

	~SanitySystem()
    {
        eventBrokerComponent.Unsubscribe<SanityEvents.ChangeSanity>(ChangeSanityHandler);
		eventBrokerComponent.Unsubscribe<SanityEvents.GetSanity>(GetSanityHandler);
    }

    private void ChangeSanityHandler(BrokerEvent<SanityEvents.ChangeSanity> inEvent)
    {
		SanityType sanityType = inEvent.Payload.SanityType;

		if (sanityType == SanityType.Neutral) return;
		int change = 0;
		if (sanityType == SanityType.Negative)
		{
			change = -1;
		} else if (sanityType == SanityType.Positive)
		{
			change = 1;
		}
		currentSanity = Mathf.Clamp(currentSanity + change, MinimumSanityLevel, MaximumSanityLevel);
    }

	private void GetSanityHandler(BrokerEvent<SanityEvents.GetSanity> inEvent)
	{
		inEvent.Payload.ProcessData?.Invoke(sanityCurve.Evaluate(currentSanity));
	}
}
