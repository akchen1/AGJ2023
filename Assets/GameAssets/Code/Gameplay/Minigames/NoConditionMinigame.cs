using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NoConditionMinigame : MonoBehaviour, IMinigame
{
    [SerializeField] private GameObject minigameUI;

    [SerializeField] private UnityEvent OnMinigameFinish;

    private bool active = false;
    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();
    public void Finish()
    {
        active = false;
        minigameUI.SetActive(false);

        this.EndMinigame();
        OnMinigameFinish?.Invoke();
        //eventBrokerComponent.Publish(this, new InputEvents.SetInputState(true));
    }

    public void Initialize()
    {
        active = true;
        minigameUI.SetActive(true);
        //eventBrokerComponent.Publish(this, new InputEvents.SetInputState(false));
    }

    public bool StartCondition()
    {
        return true;
    }
    
    public void CloseMinigame()
    {
        Finish();
    }
}
