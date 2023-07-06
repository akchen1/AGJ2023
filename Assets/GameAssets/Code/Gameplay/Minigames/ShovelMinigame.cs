using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class ShovelMinigame : MonoBehaviour, IMinigame, IPointerClickHandler
{
    private bool active;
    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

    [SerializeField] private BarSliderUI barSlider;
    
    [SerializeField] private List<ShovelLevels> levels;
    [SerializeField] private List<Animator> mudAnimators;
    [SerializeField] private Animator shovelAnimator;
    [SerializeField] private int currentLevel = 0;
    [SerializeField] private float currentProgress;
    [SerializeField] private List<InventoryItem> requiredItems;
    [SerializeField] private List<InventoryItem> endMinigameItems; // Items to give the player upon finishing the minigame

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
        barSlider.VerticalBarSpeed = levels[0].barSpeed;
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
        HandleInventoryEvents();
        this.EndMinigame();
    }
    #endregion

    private void MouseClickHandler(BrokerEvent<InputEvents.MouseClick> obj)
    {
        //if (!active || !barSlider.Active) return;

        //StartCoroutine(PauseSlider(1.5f));
        //float value = GetSliderValue();
        //DecreaseProgress(value);
        //if (!CheckIfNextLevel()) return;
        //IncreaseLevel();
        //PlayAnimation();
        //if (!CheckEndCondition()) return;
        //StartCoroutine(DelayedFinish());
    }

    private bool CheckEndCondition()
    {
        return currentLevel >= levels.Count;
    }

    private void PlayAnimation()
    {
        mudAnimators[currentLevel - 1].SetTrigger("Mud");
        shovelAnimator.SetTrigger("Shovel" + (currentLevel));
    }

    private void IncreaseLevel()
    {
        currentLevel++;
        if (currentLevel >= levels.Count) return;
        currentProgress = levels[currentLevel].levelRequirement;
        barSlider.VerticalBarSpeed = levels[currentLevel].barSpeed;
    }

    private bool CheckIfNextLevel()
    {
        return currentProgress <= 0;
    }

    private void DecreaseProgress(float value)
    {
        currentProgress -= value;
    }

    private float GetSliderValue()
    {
        Vector2 displacement = barSlider.GetSliderDisplacement();
        return levels[currentLevel].levelCurve.Evaluate(displacement.y);

    }

    private IEnumerator PauseSlider(float seconds)
    {
        barSlider.Active = false;
        yield return new WaitForSeconds(seconds);
        barSlider.Active = true;
    }

    private IEnumerator DelayedFinish()
    {
        active = false;
        barSlider.Active = false;
        yield return new WaitForSeconds(2f);
        Finish();
    }

    public void OnPointerClick(PointerEventData eventData)
    {
        if (!active || !barSlider.Active) return;

        StartCoroutine(PauseSlider(1.5f));
        float value = GetSliderValue();
        DecreaseProgress(value);
        if (!CheckIfNextLevel()) return;
        IncreaseLevel();
        PlayAnimation();
        if (!CheckEndCondition()) return;
        StartCoroutine(DelayedFinish());
    }
    private void HandleInventoryEvents()
    {
        eventBrokerComponent.Publish(this, new InventoryEvents.AddItem(endMinigameItems.ToArray()));
        eventBrokerComponent.Publish(this, new InventoryEvents.RemoveItem(requiredItems.ToArray()));
    }
    [System.Serializable]
    private class ShovelLevels
    {
        public float levelRequirement;
        public AnimationCurve levelCurve;
        public float barSpeed;
    }
}
