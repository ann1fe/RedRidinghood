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
    private float normalSpeed;
    

    void Start()
    {
        controller = GetComponent<CharacterController>();
        normalSpeed = speed;
    }

    void Update()
    {
        // Get input for horizontal and vertical movement (WASD or arrow keys)
        float rightSpeed = Input.GetAxis("Horizontal") * speed;
        float forwardSpeed = Input.GetAxis("Vertical") * speed;

        // controller.Move requires a movement vector in world coordinates.
        // To achieve this, we calculate movement in the player's local space and transform it into world coordinates:
        // - Multiply rightSpeed by the player's right vector (transform.right) to calculate the rightward movement in world space.
        // - Multiply forwardSpeed by the player's forward vector (transform.forward) to calculate the forward movement in world space.
        // Additionally, we add a downward velocity to simulate gravity, using Vector3.down multiplied by a constant gravity value.
        // finally we add all 3 movement vectors to get player movement vector in the world coordinates
        Vector3 moveSpeed = transform.right * rightSpeed + transform.forward * forwardSpeed + Vector3.down * gravity;
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

}
