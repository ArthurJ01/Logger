using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Pickup : MonoBehaviour
{
    [SerializeField] private LayerMask logLayer;

    private PlayerInputActions inputActions;

    private CarryingLogs carryingLogs;

    private bool inPickupRange = false;
    private GameObject currentLog = null;


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

    public void OnPickup(InputAction.CallbackContext context)
    {
        bool carrySuccess = false;
        if (inPickupRange && currentLog != null)
        {
            carrySuccess = carryingLogs.AddLogToCarried();
        }
        if (carrySuccess)
        {
            currentLog.GetComponent<Log>().PickupLog();
            carrySuccess = false;
        }
        
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & logLayer) != 0)
        {
            inPickupRange = true;
            currentLog = other.gameObject;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & logLayer) != 0)
        {
            inPickupRange = false;
            currentLog = null;
        }
    }


}
