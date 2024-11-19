using UnityEngine;
using UnityEngine.InputSystem;

public class CameraMovement : MonoBehaviour
{
    public Transform player; // Reference to the player's Transform
    public float rotationSpeed = 100f; // Speed of rotation

    private float horizontalInput; // Input for camera rotation
    private PlayerInputActions inputActions;

    private void Awake()
    {
        // Initialize the input actions
        inputActions = new PlayerInputActions();
    }

    private void OnEnable()
    {
        // Enable the input actions and subscribe to the RotateCamera action
        inputActions.Player.Enable();
        inputActions.Player.RotateCamera.performed += OnRotateCameraPerformed;
        inputActions.Player.RotateCamera.canceled += OnRotateCameraCanceled;
    }

    private void OnDisable()
    {
        // Unsubscribe and disable the input actions
        inputActions.Player.RotateCamera.performed -= OnRotateCameraPerformed;
        inputActions.Player.RotateCamera.canceled -= OnRotateCameraCanceled;
        inputActions.Player.Disable();
    }

    private void OnRotateCameraPerformed(InputAction.CallbackContext context)
    {
        // Read input value when the action is performed
        horizontalInput = context.ReadValue<float>();
    }

    private void OnRotateCameraCanceled(InputAction.CallbackContext context)
    {
        // Reset input value when the action is canceled (key released)
        horizontalInput = 0f;
    }

    void Update()
    {
        if (Mathf.Abs(horizontalInput) > 0.01f) // Only rotate if there's input
        {
            // Calculate the rotation angle
            float rotationAmount = horizontalInput * rotationSpeed * Time.deltaTime;

            // Rotate around the player
            transform.RotateAround(player.position, Vector3.up, rotationAmount);

        }
    }
}
