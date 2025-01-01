using UnityEngine;
using System.Collections; // For IEnumerator


/// <summary>
/// Ends the game when player collides with KillZone. 
/// Additionally when attached to the Enemy, Killzone rotates the player towards the enemy before ending the game
/// </summary>
public class Killzone : MonoBehaviour
{
    public float rotationSpeed = 5; // Speed of the rotation
    void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger is the player
        if (other.CompareTag("Player"))
        {
            if (gameObject.CompareTag("Enemy")) {
                // Start rotating the player towards the enemy smoothly
                StartCoroutine(RotateTowardsEnemy(other.gameObject));      
            } 
            else 
            {
                GameManager.Instance.GameOver();
            }
        }
    }
    
    /// <summary>
    /// Coroutine which rotates the player towards the enemy when they collide with each other
    /// Otherwise the player might not know why they died
    /// </summary>
    IEnumerator RotateTowardsEnemy(GameObject player)
    {
        GameManager.Instance.SetPlayerFrozen(true);
        // Calculate direction from player to enemy
        Vector3 directionToEnemy = gameObject.transform.position - player.transform.position;
        directionToEnemy.y = 0; // Keep the rotation only on the Y-axis

        // Calculate the target rotation
        Quaternion targetRotation = Quaternion.LookRotation(directionToEnemy);

        // Smoothly rotate towards the target
        while (Quaternion.Angle(player.transform.rotation, targetRotation) > 1f)
        {
            var newRot = Quaternion.Slerp(player.transform.rotation, targetRotation, Time.deltaTime * rotationSpeed);
            player.transform.rotation = newRot;
            yield return null; // Leaves the coroutine to do other work and return here again on next frame
        }
        
        // Ensure exact rotation at the end
        player.transform.rotation = targetRotation;
        GameManager.Instance.GameOver();
    }
}
