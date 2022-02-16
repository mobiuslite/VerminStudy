using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BigHatRat_Script : MonoBehaviour
{
    [SerializeField]
    Dialogue dialogue;

    public void TriggerDialogue()
    {
        FindObjectOfType<DialogueManager>().StartDialogue(dialogue);
    }

    private void OnCollisionEnter(Collision collision)
    {
        if (collision.gameObject.CompareTag("Player"))
        {
            TriggerDialogue();
        }
    }
}
