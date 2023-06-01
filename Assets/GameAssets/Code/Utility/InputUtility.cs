using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class InputUtility
{
    public static Vector2 ScreenToWorldPoint(this Vector2 screenPosition)
    {
        Camera cam = Camera.main;
        return cam.ScreenToWorldPoint(screenPosition);
    }
}
