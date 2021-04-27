using UnityEngine;
using TMPro;

public class Sign: Interactable
{
    public GameObject dialogueBox;
    public TextMeshProUGUI dialogueText;
    public string dialogue;

    // Update is called once per frame
    public virtual void Update()
    {
        if (Input.GetButtonDown("Interact") && playerInRange)
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
