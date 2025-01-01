using UnityEngine;
using TMPro;

public class CollectableItem : MonoBehaviour
{
    private bool playerInRange = false;

    public TextMeshProUGUI collectPrompt;

    void Start()
    {
        collectPrompt.gameObject.SetActive(false);
        var manager = FindObjectOfType<GameManager>();
        manager.RegisterMushroom();
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
        GameManager.Instance.CollectMushroom();
        collectPrompt.gameObject.SetActive(false);

        Destroy(gameObject);
    }
}