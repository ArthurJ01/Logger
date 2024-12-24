using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Interact : MonoBehaviour
{
    [SerializeField] private LayerMask interactableLayer;
    

    private PlayerInputActions inputActions;
    private Inventory inventory;

    private bool inPickupRange = false;
    private GameObject currentInteractableObj = null;

    private List<GameObject> interactableObjects = new List<GameObject>();


    private void Start()
    {
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
        inputActions.Player.Pickup.performed += OnInteract;
    }

    private void OnDisable()
    {
        // Unsubscribe and disable the input actions
        inputActions.Player.Pickup.performed -= OnInteract;
        inputActions.Player.Disable();
    }

    public void OnInteract(InputAction.CallbackContext context)
    {
        if (interactableObjects.Count > 0)
        {
            // Pick the first object in the list
            GameObject objectToPickup = interactableObjects[0];
            if (objectToPickup.TryGetComponent<IInteractable>(out IInteractable component))
            {
                GameObject current = component.Interact();
                component.MakePickedUpState();
                inventory.AddToContainer(current);
                interactableObjects.Remove(objectToPickup);
            }
        }
        else
        {
            GameObject objectToDrop = inventory.RetrieveFromContainer();
            DropItem(objectToDrop);

        }
    }


    private void DropItem(GameObject objectToDrop)
    {
        if(objectToDrop == null)
        {
            Debug.Log("nothing in inventory");
            return;
        }

        Debug.Log("dropping: " + objectToDrop);
        objectToDrop.TryGetComponent<IInteractable>(out IInteractable component);
        component.MakeDroppedState();

        objectToDrop.transform.SetParent(null);
        objectToDrop.transform.position = this.gameObject.transform.position;

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
