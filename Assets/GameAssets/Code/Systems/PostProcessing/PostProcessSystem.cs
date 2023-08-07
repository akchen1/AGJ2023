using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Rendering;
using UnityEngine.Rendering.Universal;


public class PostProcessSystem : MonoBehaviour
{
    public Volume postProcessingVolume;
    private Vignette vignette;

    private float target;

    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

    private void Awake()
    {
        postProcessingVolume.profile.TryGet(out vignette);
    }

    private void OnEnable()
    {
        eventBrokerComponent.Subscribe<PostProcessingEvents.SetVignette>(SetVignetteHandler);
    }

    private void OnDisable()
    {
        eventBrokerComponent.Unsubscribe<PostProcessingEvents.SetVignette>(SetVignetteHandler);

    }

    private void SetVignetteHandler(BrokerEvent<PostProcessingEvents.SetVignette> inEvent)
    {
        target = inEvent.Payload.Intensity;
        StartCoroutine(FadeVignette(target));
    }

    private IEnumerator FadeVignette(float target)
    {
        float time = 1f;
        float step = (target - vignette.intensity.value) / time;

        for (float i = 0; i < time; i += Time.deltaTime)
        {
            vignette.intensity.value += step * Time.deltaTime;
            yield return null;
        }
        vignette.intensity.value = target;
    }
}
