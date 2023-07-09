using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class DistractionMinigame : MonoBehaviour, IMinigame
{
    private bool finished;
    private float waitTime;
    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();
    [SerializeField] GameObject distractionMinigamePrefab;
    [SerializeField] Canvas canvas;
    [SerializeField, Header("Cutscene")] private PlayableAsset minigameStartCutscene;
     [SerializeField] private PlayableAsset hideCutscene;
    [SerializeField]private PlayableAsset shopKeeperCutscene;
    [SerializeField] private PlayableDirector playableDirector;
    [SerializeField] private Image fadeToBlack;
    [SerializeField, Header("UI")] private GameObject panel;
    

    #region IMinigame Methods
    public void Finish()
    {
        fadeToBlack.color = new Color(fadeToBlack.color.r, fadeToBlack.color.g, fadeToBlack.color.b, 1f);
        fadeToBlack.gameObject.SetActive(true);
        this.EndMinigame();
        playableDirector.Play(shopKeeperCutscene);
        panel.SetActive(false);
    }

    public void Initialize()
    {
        playableDirector.Play(minigameStartCutscene);
        panel.SetActive(true);
    }

    public bool StartCondition()
    {
        return true;
    }
    #endregion

    private void StartSequence(BrokerEvent<DistractionEvent.Start> inEvent)
    {
        finished = false;
        StartCoroutine(WaitSequence(inEvent));
    }
    private void Finished(BrokerEvent<DistractionEvent.Finished> inEvent)
    {   finished = true;
        Finish();
    }
    private IEnumerator WaitSequence(BrokerEvent<DistractionEvent.Start> inEvent)
    {
        playableDirector.Play(hideCutscene);
        float elapsedTime = 0f;
        yield return null;
        while(elapsedTime < waitTime)
        {
            elapsedTime += Time.deltaTime;
            yield return null;
        }
        if(!finished)
        {
            StartCoroutine(ResetMinigame());
        }
    }
    private void OnEnable() {
        eventBrokerComponent.Subscribe<DistractionEvent.Start>(StartSequence);
        eventBrokerComponent.Subscribe<DistractionEvent.Finished>(Finished);
        eventBrokerComponent.Subscribe<DistractionTimerEvent.SetDistracitonTime>(SetWaitTimer);
    }

    private void SetWaitTimer(BrokerEvent<DistractionTimerEvent.SetDistracitonTime> inEvent)
    {
        waitTime = inEvent.Payload.DistractionTime;
    }

    private void OnDisable() {
        eventBrokerComponent.Unsubscribe<DistractionEvent.Start>(StartSequence);
        eventBrokerComponent.Unsubscribe<DistractionEvent.Finished>(Finished);
        eventBrokerComponent.Unsubscribe<DistractionTimerEvent.SetDistracitonTime>(SetWaitTimer);
    }

    private IEnumerator ResetMinigame()
    {
        fadeToBlack.color = new Color(fadeToBlack.color.r, fadeToBlack.color.g, fadeToBlack.color.b, 0);
        fadeToBlack.gameObject.SetActive(true);

        while (fadeToBlack.color.a < 1f)
        {
            fadeToBlack.color = new Color(fadeToBlack.color.r, fadeToBlack.color.g, fadeToBlack.color.b, fadeToBlack.color.a + (Time.deltaTime * 2f));
            yield return null;
        }

        fadeToBlack.color = new Color(fadeToBlack.color.r, fadeToBlack.color.g, fadeToBlack.color.b, 1f);

        Destroy(panel.gameObject);
        GameObject instantiatedObject = Instantiate(distractionMinigamePrefab, canvas.transform);
        instantiatedObject.transform.SetAsFirstSibling();
        panel = instantiatedObject;

        yield return new WaitForSeconds(1f);

        while (fadeToBlack.color.a > 0f)
        {
          fadeToBlack.color = new Color(fadeToBlack.color.r, fadeToBlack.color.g, fadeToBlack.color.b, fadeToBlack.color.a - (Time.deltaTime * 2f));
          yield return null;
        }

        fadeToBlack.color = new Color(fadeToBlack.color.r, fadeToBlack.color.g, fadeToBlack.color.b, 0f);
        fadeToBlack.gameObject.SetActive(false);
	  }
}
