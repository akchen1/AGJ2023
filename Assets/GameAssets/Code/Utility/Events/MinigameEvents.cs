using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class MinigameEvents
{
    public class StartMinigame
    {
        public readonly IMinigame minigame;

        public StartMinigame(IMinigame minigame)
        {
            this.minigame = minigame;
        }
    }

    public class EndMinigame
    {

    }
}
