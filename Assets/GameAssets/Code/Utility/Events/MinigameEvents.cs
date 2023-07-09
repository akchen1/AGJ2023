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
        public readonly bool Completed;
        public EndMinigame(IMinigame minigame, bool completed = true)
        {
            Minigame = minigame;
            Completed = completed;
        }
    }
}
