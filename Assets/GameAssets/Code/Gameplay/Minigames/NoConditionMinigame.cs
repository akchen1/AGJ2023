using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class NoConditionMinigame : MonoBehaviour, IMinigame
{
    [SerializeField] private GameObject minigameUI;

    public UnityEvent OnMinigameFinish;

    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();
    public void Finish()
    {
        minigameUI.SetActive(false);

        this.EndMinigame();
        OnMinigameFinish?.Invoke();
    }

    public void Initialize()
    {
        minigameUI.SetActive(true);
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
