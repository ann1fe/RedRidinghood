using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mud : MonoBehaviour
{
    public float mudSpeed = 2f; // Movement speed on mud

    private void OnTriggerEnter(Collider other)
    {
        // Check if the Player entered the mud
        if (other.CompareTag("Player"))
        {
            // Access the PlayerMovement script and change the speed
            PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.SetMovementSpeed(mudSpeed);
                playerMovement.SetCanJump(false);
            }
        }
    }

    private void OnTriggerExit(Collider other)
    {
        // Check if the Player exited the mud
        if (other.CompareTag("Player"))
        {
            // Reset the player's speed to normal
            PlayerMovement playerMovement = other.GetComponent<PlayerMovement>();
            if (playerMovement != null)
            {
                playerMovement.ResetMovementSpeed();
                playerMovement.SetCanJump(true);
            }
        }
    }
}
