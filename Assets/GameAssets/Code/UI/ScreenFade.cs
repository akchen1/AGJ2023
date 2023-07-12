using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class ScreenFade : MonoBehaviour
{
    private Image fadeImage;

    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

    private void Start()
    {
        fadeImage = GetComponent<Image>();
    }

    private void OnEnable()
    {
        eventBrokerComponent.Subscribe<TransitionEvents.FadeScreen>(FadeScreenHandler);      
    }

    private void OnDisable()
    {
        eventBrokerComponent.Unsubscribe<TransitionEvents.FadeScreen>(FadeScreenHandler);
    }

    private void FadeScreenHandler(BrokerEvent<TransitionEvents.FadeScreen> obj)
    {
        float fadeDurationSeconds = obj.Payload.FadeDurationSeconds;
        if (obj.Payload.FadeIn)
        {
            StartCoroutine(FadeIn(fadeDurationSeconds));
        } else
        {
            StartCoroutine(FadeOut(fadeDurationSeconds));
        }
    }

    private IEnumerator FadeOut(float fadeDurationSeconds)
    {
        Color color = fadeImage.color;
        if (fadeDurationSeconds == 0)
        {
            color.a = 1;
            fadeImage.color = color;
            yield break;
        }

        for (float t = fadeDurationSeconds; t > 0; t -= Time.deltaTime)
        {
            color.a = Mathf.Lerp(0f, 1f, t / fadeDurationSeconds); // 1 to 0
            fadeImage.color = color;
            yield return null;
        }
        color.a = 0;
        fadeImage.color = color;
    }

    private IEnumerator FadeIn(float fadeDurationSeconds)
    {
        Color color = fadeImage.color;
        if (fadeDurationSeconds == 0)
        {
            color.a = 0;
            fadeImage.color = color;
            yield break;
        }
        for (float t = 0; t < fadeDurationSeconds; t += Time.deltaTime)
        {
            color.a = Mathf.Lerp(0f, 1f, t/fadeDurationSeconds); // 0 to 1
            fadeImage.color = color;
            yield return null;
        }
        color.a = 1;
        fadeImage.color = color;
    }
}
