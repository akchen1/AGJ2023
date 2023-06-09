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
    private bool canInteract;
    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();
    [SerializeField] GameObject distractionMinigamePrefab;
    [SerializeField] Canvas canvas;
    [SerializeField, Header("Cutscene")] private PlayableAsset hideCutscene;
    [SerializeField] private PlayableDirector playableDirector;
    [SerializeField] private Image fadeToBlack;
    [SerializeField, Header("UI")] private GameObject panel;
    

    #region IMinigame Methods
    public void Finish()
    {
        fadeToBlack.color = new Color(fadeToBlack.color.r, fadeToBlack.color.g, fadeToBlack.color.b, 1f);
        fadeToBlack.gameObject.SetActive(true);
        eventBrokerComponent.Publish(this, new MinigameEvents.EndMinigame());
    }

    public void Initialize()
    {
        panel.SetActive(true);
    }

    public bool StartCondition()
    {
        if(canInteract)
            return true;
        else
            return false;
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
        eventBrokerComponent.Subscribe<InputEvents.SetInputState>(GetInputStateHandler);
    }

    private void SetWaitTimer(BrokerEvent<DistractionTimerEvent.SetDistracitonTime> inEvent)
    {
        waitTime = inEvent.Payload.DistractionTime;
    }

    private void OnDisable() {
        eventBrokerComponent.Unsubscribe<DistractionEvent.Start>(StartSequence);
        eventBrokerComponent.Unsubscribe<DistractionEvent.Finished>(Finished);
        eventBrokerComponent.Unsubscribe<DistractionTimerEvent.SetDistracitonTime>(SetWaitTimer);
        eventBrokerComponent.Unsubscribe<InputEvents.SetInputState>(GetInputStateHandler);
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

    private void GetInputStateHandler(BrokerEvent<InputEvents.SetInputState> inEvent)
    {
        canInteract =  inEvent.Payload.Active;
    }
}
