using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameEvents
{
    public class StartMinigame
    {
        public readonly IMinigame Minigame;

        public StartMinigame(IMinigame minigame)
        {
            Minigame = minigame;
        }
    }

    public class EndMinigame
    {
        public readonly IMinigame Minigame;
        public EndMinigame(IMinigame minigame)
        {
            Minigame = minigame;
        }
    }
}
