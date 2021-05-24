using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagingTileScript : MonoBehaviour
{
    GridAreaScript gridScript;
    private BoxCollider2D hitbox;

    private Coroutine coroutine;
    private float timer = 0;
    private float activationTimer = 0;
    public float deactivationDelay = 0.3f;
    public float activationDelay = 2f;
    private bool isFinalTile = false;
    private Color originalColour;
    private Animator animator;
    [SerializeField]private AudioSource audio;

    public SpriteRenderer sr;
    // Start is called before the first frame update
    void Start()
    {
        originalColour = sr.color;
        gridScript = GameObject.FindWithTag("GridArea").GetComponent<GridAreaScript>();
        hitbox = GetComponentInChildren<BoxCollider2D>();
        hitbox.enabled = false;
        animator = GetComponent<Animator>();
        audio = GetComponentInParent<AudioSource>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void activateTile() {
        gameObject.SetActive(true);

    }

    void deactivateTile() {

        gameObject.SetActive(false);
        animator.SetBool("isActivated", false);

        //Send signal to script that the final tile has activated
        if (isFinalTile) {
            //Debug.Log("FINAL TILE");
            isFinalTile = false;
            gridScript.isCasting = false;

        }
    }

    public void setFinalTile() {
        isFinalTile = true;
    }

    public void playAudio() {
        audio.Play();
    }

}
