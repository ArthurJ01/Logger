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
        objectToDrop.transform.position = this.gameObject.transform.position + transform.forward;

        Vector3 localForceDirection = new Vector3(0, 4, 1).normalized; // Local 45-degree angle

        // Convert the local direction to world space
        Vector3 worldForceDirection = transform.TransformDirection(localForceDirection);

        // Calculate the force vector
        Vector3 force = worldForceDirection * 1;

        objectToDrop.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);

        Debug.Log($"Force applied: {force}, Position: {objectToDrop.transform.position}");


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
