using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Pickup : MonoBehaviour
{
    [SerializeField] private LayerMask interactableLayer;
    

    private PlayerInputActions inputActions;

    private CarryingLogs carryingLogs;
    private Inventory inventory;

    private bool inPickupRange = false;
    private GameObject currentInteractableObj = null;


    private void Start()
    {
        carryingLogs = this.gameObject.GetComponent<CarryingLogs>();
        inventory = this.gameObject.GetComponent<Inventory>();
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
        inputActions.Player.Pickup.performed += OnPickupPress;
    }

    private void OnDisable()
    {
        // Unsubscribe and disable the input actions
        inputActions.Player.Pickup.performed -= OnPickupPress;
        inputActions.Player.Disable();
    }

    public void OnPickupPress(InputAction.CallbackContext context)
    {

        if (inPickupRange && currentInteractableObj != null)
        {
            if (currentInteractableObj.TryGetComponent<IInteractable>(out IInteractable component))
            {
                GameObject current = component.Interact();
                component.MakePickedUpState();
                inventory.AddToContainer(current);
                inPickupRange = false;
                currentInteractableObj = null;
            }

            
        }
                  
        /*
        bool carrySuccess = false;
        if (inPickupRange && currentInteractableObj != null)
        {
            if (currentInteractableObj.TryGetComponent<IInteractable>(out IInteractable component))
            {
                component.Interact();
            }

            carrySuccess = carryingLogs.AddLogToCarried();
        }
        if (carrySuccess)
        {
            GameObject returned = currentInteractableObj.GetComponent<Log>().PickupLog();
            Debug.Log(returned);
            carrySuccess = false;
        }
        */

    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & interactableLayer) != 0)
        {
            inPickupRange = true;
            currentInteractableObj = other.gameObject;
        }
    }
    private void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & interactableLayer) != 0)
        {
            inPickupRange = false;
            currentInteractableObj = null;
        }
    }


}
