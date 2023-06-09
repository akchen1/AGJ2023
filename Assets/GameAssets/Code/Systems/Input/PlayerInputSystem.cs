using System;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class PlayerInputSystem
{
    private PlayerInputActions inputActions = new PlayerInputActions();

    private EventBrokerComponent eventBrokerComponent = new EventBrokerComponent();
    public PlayerInputSystem()
    {
        eventBrokerComponent.Subscribe<InputEvents.SetInputState>(SetInputStateHandler);
        eventBrokerComponent.Subscribe<InputEvents.GetMousePosition>(GetMousePositionHandler);
        inputActions.Player.MouseClick.performed += OnMouseClick;
        EnableInput();
    }

    ~PlayerInputSystem()
    {
        eventBrokerComponent.Unsubscribe<InputEvents.SetInputState>(SetInputStateHandler);
        eventBrokerComponent.Unsubscribe<InputEvents.GetMousePosition>(GetMousePositionHandler);
        inputActions.Player.MouseClick.performed -= OnMouseClick;
        DisableInput();
    }

    private void GetMousePositionHandler(BrokerEvent<InputEvents.GetMousePosition> inEvent)
    {
        inEvent.Payload.Position?.Invoke(inputActions.Player.MousePosition.ReadValue<Vector2>());
    }

    private void SetInputStateHandler(BrokerEvent<InputEvents.SetInputState> inEvent)
    {
        if (inEvent.Payload.Active)
        {
            EnableInput();
        }
        else
        {
            DisableInput();
        }
    }

    private void EnableInput()
    {
        inputActions.Enable();
    }

    private void DisableInput()
    {
        inputActions.Disable();
    }

    private void OnMouseClick(InputAction.CallbackContext context)
    {
        Vector2 mousePosition = inputActions.Player.MousePosition.ReadValue<Vector2>();
        eventBrokerComponent.Publish(this, new InputEvents.MouseClick(mousePosition));
    }
}
