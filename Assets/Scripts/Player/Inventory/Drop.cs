using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class Drop : MonoBehaviour
{
    private PlayerInputActions inputActions;
    private Inventory inventory;

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

    public void DropItem(InputAction.CallbackContext context)
    {
        Debug.Log("dropping");

        GameObject objectToDrop = inventory.RetrieveFromContainer();

        if (objectToDrop == null)
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
}
