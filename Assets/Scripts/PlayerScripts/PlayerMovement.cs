using System.Collections;
using System.Collections.Generic;
using System.Numerics;
using UnityEngine;
using Vector3 = UnityEngine.Vector3;

public class PlayerMovement : MonoBehaviour
{
    private CharacterController controller;
    public float speed = 5f;
    public float gravity = 9.8f;
    public float jumpHeight = 2f;
    private bool canJump = true;
    private float upSpeed;
    public float groundCheckDistance = 0.4f;
    public LayerMask groundMask;
    private float normalSpeed;
    

    void Start()
    {
        controller = GetComponent<CharacterController>();
        normalSpeed = speed;
    }
    bool IsGrounded()
    {
        Vector3 capsuleBottom = transform.position - Vector3.up * controller.height / 2;
        return Physics.CheckSphere(capsuleBottom, groundCheckDistance, groundMask);    
    }
    void Update()
    {
        var isGrounded = IsGrounded();

        // Get input for horizontal and vertical movement (WASD or arrow keys)
        float rightSpeed = Input.GetAxis("Horizontal") * speed;
        float forwardSpeed = Input.GetAxis("Vertical") * speed;

        // Jump logic
        if (isGrounded && canJump && Input.GetButtonDown("Jump"))
        {
            // v2=2gh => v = sqrt(2gh)
            upSpeed = Mathf.Sqrt(2f * gravity * jumpHeight);
        }

        // apply gravity to vertical speed v=g*t
        upSpeed -= gravity * Time.deltaTime;
        Vector3 moveSpeed = transform.right * rightSpeed + transform.forward * forwardSpeed + Vector3.up * upSpeed;
        controller.Move(moveSpeed * Time.deltaTime);

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
