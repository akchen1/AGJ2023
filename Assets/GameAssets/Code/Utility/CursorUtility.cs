using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

public class CursorUtility : MonoBehaviour
{
    [SerializeField] private Texture2D defaultCursor;
    [SerializeField] private Texture2D interactCursor;
    [SerializeField] private Texture2D enterCursor;
    [SerializeField] private EventSystem eventSystem;

    private int UILayer;
    private int interactablesLayer;

    void Start()
    {
        UILayer = LayerMask.NameToLayer("UI");
        interactablesLayer = LayerMask.NameToLayer("Interactables");
        SetCursor(defaultCursor);
    }

    private void OnGUI()
    {
        if (Event.current.type != EventType.Repaint) return;

        PointerEventData pointerEventData = new PointerEventData(eventSystem);
        Vector2 position = Event.current.mousePosition;
        position.y = Screen.height - position.y;
        pointerEventData.position = position;

        List<RaycastResult> results = new List<RaycastResult>();
        eventSystem.RaycastAll(pointerEventData, results);

        bool canvasInteractableDetected = false;
        bool canvasNonInteractableDetected = false;
        bool worldInteractableDetected = false;
        bool sceneChangeInteractableDetected = false;
        foreach (RaycastResult result in results)
        {
            // 1. Check if UI interactable layer is hit
            // 2. Check if UI non-interactable layer is hit
            // 3. Check if world object is hit
            int layer = result.gameObject.layer;
            string tag = result.gameObject.tag;
    
            if (result.gameObject.GetComponent<Image>()?.raycastTarget == true && layer == interactablesLayer)
            {
                canvasInteractableDetected = true;
            }
            if (layer == UILayer)
            {
                canvasNonInteractableDetected = true;
            }
            Collider2D worldCollider = result.gameObject.GetComponent<Collider2D>();

            if (worldCollider != null && worldCollider.enabled && layer == interactablesLayer && tag == "SceneChangeInteractable")
            {
                sceneChangeInteractableDetected = true;
            } else if (worldCollider != null && worldCollider.enabled && layer == interactablesLayer)
            {
                worldInteractableDetected = true;
            }
        }
        if (canvasInteractableDetected)
        {
            SetCursor(interactCursor);
        }
        else if (canvasNonInteractableDetected)
        {
            SetCursor(defaultCursor);
        }
        else if (worldInteractableDetected)
        {
            SetCursor(interactCursor);
        } else if (sceneChangeInteractableDetected)
        {
            SetCursor(enterCursor);
        }
        else
        {
            SetCursor(defaultCursor);
        }
    }

    private void SetCursor(Texture2D texture)
    {
        Cursor.SetCursor(texture, Vector2.zero, CursorMode.Auto);
    }
}
