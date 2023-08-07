using System;
using System.Collections;
using System.Collections.Generic;
using Unity.VisualScripting.Dependencies.Sqlite;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.PostProcessing;
using static Constants.Sanity;

[System.Serializable]
public class SanitySystem 
{
	private Vignette vignette;
	private float maxVignette = 0.4f;
	private float minVignette = 0f;

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
		SetSanityEffects();
    }

	private void GetSanityHandler(BrokerEvent<SanityEvents.GetSanity> inEvent)
	{
		inEvent.Payload.ProcessData?.Invoke(sanityCurve.Evaluate(currentSanity));
	}

	public void SetSanityEffects()
	{
		float intensity = maxVignette + ((sanityCurve.Evaluate(currentSanity) + 10f) * (minVignette - maxVignette)) / (10f + 10f);
		eventBrokerComponent.Publish(this, new PostProcessingEvents.SetVignette(intensity));
	}
}
