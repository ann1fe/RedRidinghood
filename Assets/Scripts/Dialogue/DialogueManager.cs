using System.Collections;
using System.Collections.Generic;
using UnityEngine.UI;
using UnityEngine;
using TMPro;
 
public class DialogueManager : MonoBehaviour
{
    public static DialogueManager Instance;

    public TextMeshProUGUI characterName;
    public TextMeshProUGUI dialogueArea;
 
    private Queue<DialogueLine> lines;
    
    public float typingSpeed = 0.2f;
 
    private void Awake()
    {
        if (Instance == null)
            Instance = this;
    
        lines = new Queue<DialogueLine>();
    }
 
    public void StartDialogue(Dialogue dialogue)
    {
        GameManager.Instance.SetPlayerFrozen(true);
        GameManager.Instance.SetEnemiesFrozen(true);
        lines.Clear();
 
        foreach (DialogueLine line in dialogue.dialogueLines)
        {
            lines.Enqueue(line);
        }
 
        DisplayNextDialogueLine();

        Cursor.lockState = CursorLockMode.None;
    }
 
    public void DisplayNextDialogueLine()
    {
        if (lines.Count == 0)
        {
            EndDialogue();
            return;
        }
 
        DialogueLine currentLine = lines.Dequeue();
 
        characterName.text = currentLine.character;
 
        StopAllCoroutines();
 
        StartCoroutine(TypeSentence(currentLine));
    }
 
    IEnumerator TypeSentence(DialogueLine dialogueLine)
    {
        dialogueArea.text = "";
        foreach (char letter in dialogueLine.line.ToCharArray())
        {
            dialogueArea.text += letter;
            yield return new WaitForSeconds(typingSpeed);
        }
    }
 
    void EndDialogue()
    {
        gameObject.SetActive(false);
        Cursor.lockState = CursorLockMode.Locked;
        GameManager.Instance.SetPlayerFrozen(false);
        GameManager.Instance.SetEnemiesFrozen(false);
        GameManager.Instance.NotifyDialogueEnded();
    }
}