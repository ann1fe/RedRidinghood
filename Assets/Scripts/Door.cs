using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Door : MonoBehaviour
{
    private bool playerInRange = false;
    public TextMeshProUGUI collectPrompt;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            CheckGameState();
        }
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            if (collectPrompt != null)
            {
                collectPrompt.gameObject.SetActive(true);
            }
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            if (collectPrompt != null)
            {
                collectPrompt.gameObject.SetActive(false);
            }
        }
    }

    void CheckGameState()
    {
        Debug.Log("Mushrooms Left: " + GameManager.Instance.MushroomsLeftCount());
        if (GameManager.Instance.MushroomsLeftCount() == 0)
        {
            GameManager.Instance.WinGame();
        } 
        /*else {
            GameObject[] mushrooms = GameObject.FindGameObjectsWithTag("Mushroom");
            GameObject closestMushroom = null;
            float minDistance = Mathf.Infinity;

            // Get the player's position
            GameObject player = GameObject.FindGameObjectWithTag("Player");

            foreach (GameObject mushroom in mushrooms)
            {
                float distance = Vector3.Distance(player.transform.position, mushroom.transform.position);
                if (distance < minDistance)
                {
                    minDistance = distance;
                    closestMushroom = mushroom;
                }
            }

            PlayerCamera playerCamera = FindObjectOfType<PlayerCamera>();
            playerCamera.RotateTowards(closestMushroom.transform.position);
        }*/
    }
}
