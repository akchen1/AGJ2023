using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TwinePlacementMinigame : ObjectPlacementMinigame
{
    [SerializeField] private GameObject Part2TwineUI;
    [SerializeField] private TwineRoll twineRoll;

    protected override bool CheckEndCondition()
    {
        bool baseCondition = base.CheckEndCondition();
        if (!baseCondition) return false;

        Part2TwineUI.SetActive(true);

        return baseCondition && Part2EndCondition();
    }

    private bool Part2EndCondition()
    {
        if (twineRoll.CreatedTwine == null) return false;
        if (twineRoll.CreatedTwine.Count < twineRoll.CreatedTwineLimit) return false;
        for (int i = 0; i < twineRoll.CreatedTwine.Count; i++)
        {
            ObjectPlacementPieceUI piece = twineRoll.CreatedTwine[i];
            if (!piece.IsInTargetPosition)
            {
                return false;
            }
        }
        return true;
    }
}
