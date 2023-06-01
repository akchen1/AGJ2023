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
        inputActions.Player.MouseClick.performed += OnMouseClick;
        EnableInput();
    }

    ~PlayerInputSystem()
    {
        inputActions.Player.MouseClick.performed -= OnMouseClick;
        DisableInput();
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
