using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CharacterControllerMovement : MonoBehaviour
{
    public CharacterController controller;

    public float speed = 6f;
    public float turnSmoothTime = 0.1f;
    public float gravity = -9.81f;
    public float jumpHeight = 1.5f;
    public float friction = 5f; // Friction multiplier to slow down movement
    public float rotationSpeed = 10f;

    private Vector3 velocity;
    private Vector3 currentMovement;
    private bool isGrounded;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;

    public Transform cameraTransform; // Reference to the camera's transform

    void Update()
    {
        // Ground check
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // TODO: Change to variable
        }

        // Movement input
        float horizontal = Input.GetAxisRaw("Horizontal");
        float vertical = Input.GetAxisRaw("Vertical");

        Vector3 direction = new Vector3(horizontal, 0f, vertical).normalized;

        if (direction.magnitude >= 0.1f)
        {
            // Convert input direction to camera-relative direction
            Vector3 cameraForward = cameraTransform.forward;
            Vector3 cameraRight = cameraTransform.right;

            // Keep movement on the horizontal plane
            cameraForward.y = 0f;
            cameraRight.y = 0f;

            cameraForward.Normalize();
            cameraRight.Normalize();

            Vector3 moveDir = (cameraForward * vertical + cameraRight * horizontal).normalized * speed;
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
        Ray ray = Camera.main.ScreenPointToRay(Input.mousePosition); // Ray from the camera to mouse position
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