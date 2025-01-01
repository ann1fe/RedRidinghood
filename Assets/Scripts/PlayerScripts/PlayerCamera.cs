using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// Takes care of handling camera up-down movement and player left-right rotation
/// Since camera is attached to player, the camera rotates alongside it
/// </summary>
public class PlayerCamera : MonoBehaviour
{
    public float mouseSensitivity = 100f; 
    public Transform playerBody; 

    private float xRotation = 0f; // To keep track of the camera's vertical rotation

    public float rotationSpeed = 180f; 

    void Start()
    {
        Cursor.lockState = CursorLockMode.Locked;
    }

    void Update()
    {
        float mouseX = Input.GetAxis("Mouse X") * mouseSensitivity * Time.deltaTime;
        float mouseY = Input.GetAxis("Mouse Y") * mouseSensitivity * Time.deltaTime;

        // mouseX (left-right) rotates player around it's up axis (y axis) 
        playerBody.Rotate(Vector3.up * mouseX);

        // Calculate and clamp vertical rotation to avoid flipping over
        xRotation -= mouseY;
        xRotation = Mathf.Clamp(xRotation, -90f, 90f);

        // mouseY (up-down) rotates camera up and down
        // To do that we need to rotate it around x axis (x axis is player left-right axis)
        transform.localRotation = Quaternion.Euler(xRotation, 0f, 0f);
    }
}
