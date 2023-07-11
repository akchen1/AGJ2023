using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;

public class CursorUtility : MonoBehaviour
{
    [SerializeField] private Texture2D defaultCursor;
    [SerializeField] private Texture2D interactCursor;
    [SerializeField] private Physics2DRaycaster raycaster;
    [SerializeField] private LayerMask layerMask;

	private bool inPauseMenu = false;

    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();

    void Start()
    {
        SetCursor(defaultCursor);
    }

    private void Update()
    {
		if (!inPauseMenu)
		{
			eventBrokerComponent.Publish(this, new InputEvents.GetMousePosition(position =>
			{
				Vector2 mouseWorldPosition = position.ScreenToWorldPoint();
				if (Physics2D.OverlapPoint(mouseWorldPosition, layerMask) != null)
				{
					SetCursor(interactCursor);
				}
				else
				{
					SetCursor(defaultCursor);
				}
			}));
		}
    }

    private void SetCursor(Texture2D texture)
    {
        Cursor.SetCursor(texture, Vector2.zero, CursorMode.Auto);
    }

	private void TogglePauseHandler(BrokerEvent<PauseEvents.TogglePause> inEvent)
	{
		inPauseMenu = !inPauseMenu;
	}

	public void OnPointerEnterUI()
	{
		SetCursor(interactCursor);
	}

	public void OnPointerExitUI()
	{
		SetCursor(defaultCursor);
	}

	private void OnEnable()
	{
		eventBrokerComponent.Subscribe<PauseEvents.TogglePause>(TogglePauseHandler);
	}

	private void OnDisable()
	{
		eventBrokerComponent.Unsubscribe<PauseEvents.TogglePause>(TogglePauseHandler);
	}
}
