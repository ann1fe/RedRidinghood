using UnityEngine;
using TMPro;

public class CollectableItem : MonoBehaviour
{
    private bool playerInRange = false;

    public TextMeshProUGUI collectPrompt;

    void Start()
    {
        if (collectPrompt != null)
        {
            collectPrompt.gameObject.SetActive(false);
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

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            Collect();
        }
    }

    void Collect()
    {
        if (GameManager.Instance != null)
        {
            GameManager.Instance.AddMushroom();
        }
        else
        {
            Debug.LogError("GameManager instance not found!");
        }

        if (collectPrompt != null)
        {
            collectPrompt.gameObject.SetActive(false);
        }

        Destroy(gameObject);
    }
}