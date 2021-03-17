using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class Terminal : Interactable
{
    public GameObject dialogueBox;
    public Text dialogueText;
    public string dialogue;

    // Update is called once per frame
    public virtual void Update()
    {
        if (Input.GetButtonDown("interact") && playerInRange)
        {
            if (dialogueBox.activeInHierarchy)
            {
                dialogueBox.SetActive(false);
            }
            else
            {
                dialogueBox.SetActive(true);
                dialogueText.text = dialogue;
            }
        }
    }
    public override void OnTriggerExit2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            dialogueBox.SetActive(false);
            base.OnTriggerExit2D(other);
        }
    }
}
