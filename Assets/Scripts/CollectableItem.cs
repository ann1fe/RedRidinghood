using UnityEngine;
using TMPro;

/// <summary>
/// Item which Player can pick up. When player collides and presses "E", item gets collected
/// </summary>
public class CollectableItem : MonoBehaviour
{
    private bool playerInRange = false;

    public TextMeshProUGUI collectPrompt;

    void Start()
    {
        collectPrompt.gameObject.SetActive(false);
        // each CollectableItem registers itself as a mushroom in the GameManager
        GameManager.Instance.RegisterMushroom();
    }

    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            collectPrompt.gameObject.SetActive(true); 
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            collectPrompt.gameObject.SetActive(false);
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
        GameManager.Instance.CollectMushroom(this.gameObject);
        collectPrompt.gameObject.SetActive(false);
        this.gameObject.SetActive(false);
        //Destroy(gameObject);
    }
}