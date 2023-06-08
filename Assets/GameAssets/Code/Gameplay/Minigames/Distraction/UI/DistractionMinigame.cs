using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Playables;
using UnityEngine.UI;

public class DistractionMinigame : MonoBehaviour, IMinigame
{
    private bool active;
    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();
    [SerializeField, Header("Cutscene")] private PlayableAsset part1Cutscene;
    [SerializeField] private PlayableDirector playableDirector;
    [SerializeField] private Image fadeToBlack;
    [SerializeField, Header("UI")] private GameObject panel;
    

    #region IMinigame Methods
    public void Finish()
    {
        throw new System.NotImplementedException();
    }

    public void Initialize()
    {
        playableDirector.Play(part1Cutscene);
        fadeToBlack.gameObject.SetActive(false);
        panel.SetActive(true);
        fadeToBlack.gameObject.SetActive(false);


        active = true;
    }

    public bool StartCondition()
    {
        return true;
    }
    #endregion
    private void OnEnable() {
        eventBrokerComponent.Subscribe<DistractionEvent.Start>(CheckFinish);
    }


    private void CheckFinish(BrokerEvent<DistractionEvent.Start> inEvent)
    {
        throw new NotImplementedException();
    }

    private void OnDisable() {
        eventBrokerComponent.Unsubscribe<DistractionEvent.Start>(CheckFinish);
    }
    private IEnumerator GameStarted()
    {
        yield return new WaitForSeconds(5);
    }
}
