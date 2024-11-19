using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.InputSystem;

public class CharacterControllerMovement : MonoBehaviour
{
    [SerializeField] private CharacterController controller;
    [SerializeField] private float speed = 6f;
    [SerializeField] private float turnSmoothTime = 0.1f;
    [SerializeField] private float gravity = -10f;
    [SerializeField] private float friction = 5f;
    [SerializeField] private float rotationSpeed = 10f;

    private Vector3 velocity;
    private Vector3 currentMovement;
    private bool isGrounded;
    private Vector2 inputDirection;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    public Transform cameraTransform; // Reference to the camera's transform

    private PlayerInputActions playerInputActions;

    private void Awake()
    {
        playerInputActions = new PlayerInputActions(); // Create it once
    }

    private void OnEnable()
    {
        playerInputActions.Player.Enable();
        playerInputActions.Player.Move.performed += OnMovePerformed;
        playerInputActions.Player.Move.canceled += OnMoveCanceled;
    }

    private void OnDisable()
    {
        playerInputActions.Player.Move.performed -= OnMovePerformed;
        playerInputActions.Player.Move.canceled -= OnMoveCanceled;
        playerInputActions.Player.Disable();
    }

    private void OnMovePerformed(InputAction.CallbackContext context)
    {
        inputDirection = context.ReadValue<Vector2>();
    }

    private void OnMoveCanceled(InputAction.CallbackContext context)
    {
        inputDirection = Vector2.zero;
    }

    void Update()
    {
        // Ground check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f;
        }

        // Movement
        if (inputDirection.sqrMagnitude >= 0.1f)
        {
            // Convert input direction to camera-relative direction
            Vector3 cameraForward = cameraTransform.forward;
            Vector3 cameraRight = cameraTransform.right;

            // Keep movement on the horizontal plane
            cameraForward.y = 0f;
            cameraRight.y = 0f;

            cameraForward.Normalize();
            cameraRight.Normalize();

            Vector3 moveDir = (cameraForward * inputDirection.y + cameraRight * inputDirection.x).normalized * speed;
            currentMovement = moveDir; // Set the current movement vector
        }
        else
        {
            currentMovement = Vector3.Lerp(currentMovement, Vector3.zero, friction * Time.deltaTime);
        }

        // Apply movement
        controller.Move(currentMovement * Time.deltaTime);

        // Apply gravity
        velocity.y += gravity * Time.deltaTime;
        controller.Move(velocity * Time.deltaTime);

        // Rotate the player to face the mouse cursor
        Ray ray = Camera.main.ScreenPointToRay(Mouse.current.position.ReadValue()); // Ray from the camera to mouse position
        if (Physics.Raycast(ray, out RaycastHit hitInfo, Mathf.Infinity, groundMask)) // Adjust groundMask to layer containing ground
        {
            Vector3 lookDirection = hitInfo.point - transform.position;
            lookDirection.y = 0f; // Keep rotation on the horizontal plane
            if (lookDirection.sqrMagnitude > 0.01f) // Avoid jittering when very close
            {
                Quaternion targetRotation = Quaternion.LookRotation(lookDirection);
                transform.rotation = Quaternion.Slerp(transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            }
        }
    }
}