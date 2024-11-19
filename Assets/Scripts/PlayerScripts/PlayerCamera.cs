using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerCamera : MonoBehaviour
{
    public float mouseSensitivity = 100f; 
    public Transform playerBody; 

    private float xRotation = 0f; // To keep track of the camera's vertical rotation

    private bool isRotatingTowardsTarget = false;
    private Quaternion targetRotation;
    public float rotationSpeed = 180f; 

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        /*if (isRotatingTowardsTarget)
        {
            // Smoothly rotate towards the target
            playerBody.rotation = Quaternion.RotateTowards(
                playerBody.rotation,
                targetRotation,
                rotationSpeed * Time.deltaTime
            );

            // Check if rotation is close enough to target rotation
            if (Quaternion.Angle(playerBody.rotation, targetRotation) < 0.1f)
            {
                // Finish rotation
                playerBody.rotation = targetRotation;
                isRotatingTowardsTarget = false;
            }
            return;
        }*/

        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        playerBody.Rotate(Vector3.up * mouseX);

        // Calculate and clamp vertical rotation to avoid flipping over
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // Apply the vertical rotation to the camera
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }

    /*public void RotateTowards(Vector3 targetPosition)
    {
        // Direction from player to target on the XZ plane
        Vector3 direction = targetPosition - playerBody.position;
        direction.y = 0f; // Ignore vertical difference

        if (direction.sqrMagnitude > 0.001f)
        {
            // Calculate the target rotation to face the target horizontally
            targetRotation = Quaternion.LookRotation(direction);

            // Only rotate on the Y-axis
            targetRotation = Quaternion.Euler(0f, targetRotation.eulerAngles.y, 0f);

            isRotatingTowardsTarget = true;
        }
    }*/
}
