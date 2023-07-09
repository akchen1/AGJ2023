using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameWrapper : MonoBehaviour
{
    [SerializeField] private bool hasExitButton;
    [SerializeField] private GameObject exitButton;
    [SerializeField] private IMinigame miniGameLogic;

    public void Initialize(IMinigame minigame)
    {
        exitButton.SetActive(true);
        miniGameLogic = minigame;
    }

    public void ExitMinigame()
    {
        if (miniGameLogic == null) return;
        miniGameLogic.Finish();
        exitButton.SetActive(false);
    }
}
