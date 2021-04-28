using UnityEngine;
using TMPro;

public class Sign: Interactable
{
    public GameObject dialogueBox;
    public TextMeshProUGUI dialogueText;
    public string dialogue;
    public GameObject player;
    private PlayerControls playerControls;
    private bool inputInteract;

    void Start()
    {
        playerControls = player.GetComponent<PlayerMovement>().getControls();
        inputInteract = false;
    }

    // Update is called once per frame
    public virtual void Update()
    {
        playerControls.Player.Interact.started += _ => inputInteract = true;

        if (inputInteract && playerInRange)
        {
            Debug.Log("input interact");
            if (dialogueBox.activeInHierarchy)
            {
                dialogueBox.SetActive(false);
            }
            else
            {
                dialogueBox.SetActive(true);
                dialogueText.text = dialogue;
            }
            inputInteract = false;
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
