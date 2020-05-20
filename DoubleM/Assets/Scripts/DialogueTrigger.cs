using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DialogueTrigger : MonoBehaviour 
{

    public Dialogue dialogue;

    public void TriggerDialogue()
    {
        if (dialogue.isUsed() && !dialogue.isLooped)
            return;
        else
        {
            FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
            dialogue.setUsed(true);
        }

    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        TriggerDialogue();
    }

    private void OnTriggerExit2D(Collider2D other)
    {
        FindObjectOfType<DialogueManager>().EndDialogue();
    }
}
