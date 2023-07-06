using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RitualMinigame : MonoBehaviour, IMinigame
{
    [SerializeField] private GameObject minigame;
    [SerializeField] private List<RitualOrder> order;

    private int currentLevel = 0;
    private bool active = false;
    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();
    public void Finish()
    {
        minigame.SetActive(false);
        active = false;
        this.EndMinigame();
    }

    public void Initialize()
    {
        minigame.SetActive(true);
        SetLevel(0);
        active = true;
    }

    public bool StartCondition()
    {
        return true;
    }

    private void Update()
    {
        if (!active) return;
        if (!CheckIfNextLevel()) return;
        currentLevel++;

        if (currentLevel >= order.Count)
        {
            active = false;
            Invoke("Finish" , 2f);
            return;
        }
        SetLevel(currentLevel);
    }

    private bool CheckIfNextLevel()
    {
        foreach (ObjectReplacementPieceUI piece in order[currentLevel].ritualItems)
        {
            if (!piece.IsInTargetPosition) return false;
        }
        return true;
    }

    private void SetLevel(int level)
    {
        foreach (ObjectReplacementPieceUI piece in order[level].ritualItems)
        {
            piece.CanPlace = true;
        }
    }

    [System.Serializable]
    public struct RitualOrder
    {
        public List<ObjectReplacementPieceUI> ritualItems;
    }
    
}
