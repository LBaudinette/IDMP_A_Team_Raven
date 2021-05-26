using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using TMPro;

public class Checkpoint : Interactable
{
    public GameObject dialogueBox;
    public TextMeshProUGUI dialogueText;
    [TextArea(15, 20)]
    public string dialogue;
    public GameObject player;
    private PlayerControls playerControls;
    private bool inputInteract;

    public PlayerInventory playerInventory;
    public FloatValue playerHealth;
    public SignalSender addPlayerHealthSignal;
    public SignalSender ReduceBoltCountSignal;

    [SerializeField] private NecromancerScript necromancerScript;
    private Vector3 startingPointVector;
    private Vector3 CheckPointVector;
    [SerializeField] private BoolValue hasCheckpointBeenHit;


    void Start()
    {
        startingPointVector = new Vector3(11, 3, 0);
        CheckPointVector = new Vector3(5, 115, 0);
        playerControls = player.GetComponent<PlayerMovement>().getControls();
        inputInteract = false;
    }

    private void FixedUpdate()
    {
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
                CheckPointActions();
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

    private void CheckPointActions()
    {
        // Reset player health
        playerHealth.runTimeValue = playerHealth.initialValue;

        // fill health potions and bolts
        playerInventory.RefillInventory();

        // player health slider signal + health potion UI signal
        addPlayerHealthSignal.Raise();

        // bolts UI signal
        ReduceBoltCountSignal.Raise();
    }

    public void CheckPointHandlePlayerDeath()
    {
        // teleport player to the before boss room or the starting point
        if (hasCheckpointBeenHit.runTimeValue)
        {
            player.transform.position = CheckPointVector;
        }
        else
        {
            player.transform.position = startingPointVector;
        }

        //reset player health and inventory
        CheckPointActions();

        // reset Necromancer health
        necromancerScript.health = 100;
    }

}
