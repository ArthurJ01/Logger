using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Pickup : MonoBehaviour
{
    private PlayerInputActions inputActions;

    private CarryingLogs carryingLogs;

    private void Start()
    {
        carryingLogs = this.gameObject.GetComponent<CarryingLogs>();
    }

    private void Awake()
    {
        // Initialize the input actions
        inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        // Enable the input actions and subscribe to the RotateCamera action
        inputActions.Player.Enable();
        inputActions.Player.Pickup.performed += OnPickup;
    }

    private void OnDisable()
    {
        // Unsubscribe and disable the input actions
        inputActions.Player.Pickup.performed -= OnPickup;
        inputActions.Player.Disable();
    }

    private void OnPickup(InputAction.CallbackContext context)
    {
        carryingLogs.addLogToCarried();
    }

}
