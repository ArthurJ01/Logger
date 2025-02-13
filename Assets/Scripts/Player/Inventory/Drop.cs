using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Drop : MonoBehaviour
{

    [SerializeField] private string containerTag;
    private PlayerInputActions inputActions;
    private Inventory inventory;

    private List<GameObject> nearbyContainers = new List<GameObject>();

    private enum DropState {Ground, Container};
    private DropState dropState = DropState.Ground;

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
        inputActions.Player.Drop.performed += DropItem;
    }

    private void OnDisable()
    {
        // Unsubscribe and disable the input actions
        inputActions.Player.Drop.performed -= DropItem;
        inputActions.Player.Disable();
    }

    private void OnTriggerEnter(Collider other)
    {
        if(other.tag == containerTag)
        {
            dropState = DropState.Container;
            nearbyContainers.Add(other.gameObject);
            Debug.Log("Container nearby");
        }
    }

    private void OnTriggerExit(Collider other)
    {
        if (other.tag == containerTag)
        {
            nearbyContainers.Remove(other.gameObject);
            Debug.Log("Leaving Container");
        }
        if (nearbyContainers.Count <= 0){
            dropState = DropState.Ground;
            Debug.Log("No Containers nearby");
        }
    }

    //drop item, either on ground or in container
    public void DropItem(InputAction.CallbackContext context)
    {
        //check nearby containers, if there is one check if it's full, and if it is, return
        //this is here because we need to do inventory.retrieve before the switch case, I think, can't find solution without
        if(nearbyContainers.Count > 0)
        {
            GameObject container = nearbyContainers[0];
            IContainer containerInterface = container.GetComponent<IContainer>();

            bool isInventoryFull = containerInterface.IsInventoryFull();

            if (isInventoryFull)
            {
                Debug.Log("inventory is full");
                return;
            }
        }

        //get object from inventory, check that there is an object
        GameObject objectToDrop = inventory.RetrieveFromContainer();
        if (objectToDrop == null)
        {
            Debug.Log("nothing in inventory");
            return;
        }

        //setup needed for both cases
        objectToDrop.TryGetComponent<IInteractable>(out IInteractable component);
        objectToDrop.transform.SetParent(null);

        switch (dropState)
        {
            case DropState.Container:

                GameObject container = nearbyContainers[0];
                component.MakePickedUpState();
                IContainer containerInterface = container.GetComponent<IContainer>();
                containerInterface.AddToContainer(objectToDrop);
                    
                break;


            case DropState.Ground:


                component.MakeDroppedState();
                objectToDrop.transform.position = this.gameObject.transform.position + transform.forward;

                Vector3 localForceDirection = new Vector3(0, 4, 1).normalized; // Local 45-degree angle

                // Convert the local direction to world space
                Vector3 worldForceDirection = transform.TransformDirection(localForceDirection);

                // Calculate the force vector
                Vector3 force = worldForceDirection * 1;

                objectToDrop.GetComponent<Rigidbody>().AddForce(force, ForceMode.Impulse);

                break;
        }
    }
}
