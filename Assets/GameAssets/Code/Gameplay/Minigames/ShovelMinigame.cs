using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ShovelMinigame : MonoBehaviour, IMinigame
{
    private bool active;
    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

    [SerializeField] private BarSliderUI barSlider;
    
    [SerializeField] private List<ShovelLevels> levels;

    [SerializeField] private int currentLevel;
    [SerializeField] private float currentProgress;

    private void OnEnable()
    {
        eventBrokerComponent.Subscribe<InputEvents.MouseClick>(MouseClickHandler);
    }

    private void OnDisable()
    {
        eventBrokerComponent.Unsubscribe<InputEvents.MouseClick>(MouseClickHandler);
    }

    #region IMinigame Methods
    public void Initialize()
    {
        currentLevel = 0;
        currentProgress = levels[0].levelRequirement;
        barSlider.gameObject.SetActive(true);
        active = true;
    }

    public bool StartCondition()
    {
        return true;
    }

    public void Finish()
    {
        barSlider.gameObject.SetActive(false);
        active = false;
        eventBrokerComponent.Publish(this, new MinigameEvents.EndMinigame());
    }
    #endregion

    private void MouseClickHandler(BrokerEvent<InputEvents.MouseClick> obj)
    {
        if (!active) return;
        Vector2 displacement = barSlider.GetSliderDisplacement();
        float val = levels[currentLevel].levelCurve.Evaluate(displacement.y);
        currentProgress -= val;
        
        if (CheckFinishCondition())
        {
            Finish();
        }
    }

    private bool CheckFinishCondition()
    {
        if (currentProgress > 0) return false;

        currentLevel++;

        if (currentLevel >= levels.Count) return true;

        currentProgress = levels[currentLevel].levelRequirement;

        return false;
    }

    [System.Serializable]
    private class ShovelLevels
    {
        public float levelRequirement;
        public AnimationCurve levelCurve;
    }
}
