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

    private List<GameObject> interactableObjects = new List<GameObject>();


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
        if (interactableObjects.Count > 0)
        {
            // Pick the first object in the list
            GameObject objectToPickup = interactableObjects[0];
            foreach (GameObject g in interactableObjects){
                Debug.Log("list object: " + g);
            }
            if (objectToPickup.TryGetComponent<IInteractable>(out IInteractable component))
            {
                GameObject current = component.Interact();
                component.MakePickedUpState();
                inventory.AddToContainer(current);
                
                // Optionally remove it from the list after picking up
                interactableObjects.Remove(objectToPickup);
            }
        }
    }

    private void OnTriggerEnter(Collider other)
    {
        if (((1 << other.gameObject.layer) & interactableLayer) != 0)
        {
            if (!interactableObjects.Contains(other.gameObject))
            {
                interactableObjects.Add(other.gameObject);
            }
            
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (((1 << other.gameObject.layer) & interactableLayer) != 0)
        {
            interactableObjects.Remove(other.gameObject);
        }
    }


}
