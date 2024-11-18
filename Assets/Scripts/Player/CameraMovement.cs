using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class CameraMovement : MonoBehaviour
{
    public Transform player; // Reference to the player's Transform
    public float rotationSpeed = 100f; // Speed of rotation

    private float currentAngle;

    void Update()
    {
        // Get input from Q and E keys
        float horizontalInput = 0f;
        if (Input.GetKey(KeyCode.Q)) horizontalInput = 1f;
        if (Input.GetKey(KeyCode.E)) horizontalInput = -1f;

        // Calculate the rotation angle
        float rotationAmount = horizontalInput * rotationSpeed * Time.deltaTime;
        currentAngle += rotationAmount;

        // Rotate around the player
        transform.position = player.position; // Keep the camera centered on the player
        transform.RotateAround(player.position, Vector3.up, rotationAmount);

        // Align camera forward vector with the player's position
        transform.LookAt(player);
    }
}
