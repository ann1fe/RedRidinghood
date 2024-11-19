using UnityEngine;

public class Killzone : MonoBehaviour
{
    void OnTriggerEnter(Collider other)
    {
        // Check if the object entering the trigger is the player
        if (other.CompareTag("Player"))
        {
            // Call the GameOver method in the GameManager
            GameManager.Instance.GameOver();
        }
    }
}
