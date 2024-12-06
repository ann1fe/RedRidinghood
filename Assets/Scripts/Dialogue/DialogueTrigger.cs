using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

[System.Serializable]
public class DialogueLine
{
    public string character;
    [TextArea(3, 10)]
    public string line;
}

[System.Serializable]
public class Dialogue
{
    public List<DialogueLine> dialogueLines = new List<DialogueLine>();
}

public class DialogueTrigger : MonoBehaviour
{
    public List<Dialogue> dialogues;
    public int dialogueIndex = 0;
    public GameObject dialogueBox;
    public TextMeshProUGUI dialoguePrompt;
    private bool playerInRange = false;

    void Update()
    {
        if (playerInRange && Input.GetKeyDown(KeyCode.E))
        {
            TriggerDialogue();
        }
    }

    public void TriggerDialogue()
    {
        dialogueBox.SetActive(true);
        int dialogueIndex;
        if (GameManager.Instance.mushroomCount == 0) {
            dialogueIndex = 0;
        } else if (GameManager.Instance.MushroomsLeftCount() != 0) {
            dialogueIndex=1;
        } else {
            dialogueIndex=2;
        }
        DialogueManager.Instance.StartDialogue(dialogues[dialogueIndex]);
    }
 
    void OnTriggerEnter(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = true;
            dialoguePrompt.gameObject.SetActive(true);
        }
    }

    void OnTriggerExit(Collider other)
    {
        if (other.CompareTag("Player"))
        {
            playerInRange = false;
            dialoguePrompt.gameObject.SetActive(false);
        }
    }
}
 