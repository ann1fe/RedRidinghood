using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    public float speed = 5f;
    public float gravity = -9.8f;
    public float jumpHeight = 2f;
    private bool canJump = true;
    private Vector3 velocity;

    public Transform groundCheck;
    public float groundDistance = 0.4f;
    public LayerMask groundMask;
    private bool isGrounded;
    private float normalSpeed;
    

    void Start()
    {
        controller = GetComponent<CharacterController>();
        normalSpeed = speed;
    }

    void Update()
    {
        // Ground check with raycast or sphere cast
        isGrounded = Physics.CheckSphere(groundCheck.position, groundDistance, groundMask);

        if (isGrounded && velocity.y < 0)
        {
            velocity.y = -2f; // Keeps the player grounded
        }

        // Get input for horizontal and vertical movement (WASD or arrow keys)
        float horizontal = Input.GetAxis("Horizontal");
        float vertical = Input.GetAxis("Vertical");

        Vector3 move = transform.right * horizontal + transform.forward * vertical;

        controller.Move(move * speed * Time.deltaTime);

        // Jump logic
        if (isGrounded && canJump && Input.GetButtonDown("Jump"))
        {
            velocity.y = Mathf.Sqrt(jumpHeight * -2f * gravity);
        }

        velocity.y += gravity * Time.deltaTime;

        controller.Move(velocity * Time.deltaTime);
    }

    // Adjust the player's movement speed
    public void SetMovementSpeed(float newSpeed)
    {
        speed = newSpeed;
    }

    // Reset the movement speed to normal
    public void ResetMovementSpeed()
    {
        speed = normalSpeed;
    }

    public void SetCanJump(bool value)
    {
        canJump = value;
    }
}
