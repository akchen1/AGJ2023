using System.Collections;
using System.Collections.Generic;
using UnityEngine;

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
}
