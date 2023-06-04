using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class PlayerEvents
{
    public class GetPlayerPosition
    {
        public GetPlayerPosition(Action<Vector3> position)
        {
            Position = position;
        }
        public readonly Action<Vector3> Position;
    }
}
