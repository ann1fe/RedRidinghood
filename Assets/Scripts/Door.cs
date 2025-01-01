using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Door : MonoBehaviour
{
    private bool playerInRange = false;
    public TextMeshProUGUI handOverPrompt;

    // Start is called before the first frame update
    void Start()
    {
        handOverPrompt.gameObject.SetActive(false);
    }

  void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            handOverPrompt.gameObject.SetActive(true); 
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            handOverPrompt.gameObject.SetActive(false);
        }
    }

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {

            GameManager.Instance.CheckEnded();
        }
    }
}
