using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using System;

public class InputEvents
{
    public class MouseClick
    {
        public readonly Vector2 MousePosition;

        public MouseClick(Vector2 mousePosition)
        {
            MousePosition = mousePosition;
        }
    }

    public class GetMousePosition
    {
        public readonly Action<Vector2> Position;
        public GetMousePosition(Action<Vector2> position)
        {
            Position = position;
        }
    }

    public class SetInputState
    {
        public readonly bool Active;

        public SetInputState(bool active)
        {
            Active = active;
        }
    }

    public class GetInputState
    {
        public GetInputState(Action<bool> allowMove)
        {
            AllowMove = allowMove;
        }
        public readonly Action<bool> AllowMove;
    }
}
