using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public static class MinigameUtility
{
    static EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

    public static bool RectOverlapsPoint(this RectTransform rectTrans1, RectTransform rectTrans2)
    {
        Rect rect1 = new Rect(rectTrans1.position.x - rectTrans1.rect.width / 2, rectTrans1.position.y - rectTrans1.rect.height / 2, rectTrans1.rect.width, rectTrans1.rect.height);
        Rect rect2 = new Rect(rectTrans2.position.x, rectTrans2.position.y, 1f, 1f);
        return rect1.Overlaps(rect2);
    }

    public static void EndMinigame(this IMinigame minigame, bool completed = true)
    {
        eventBrokerComponent.Publish(minigame, new MinigameEvents.EndMinigame(minigame, completed));
    }
}
