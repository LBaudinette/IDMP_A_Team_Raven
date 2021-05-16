using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagingTileScript : MonoBehaviour
{
    GridAreaScript gridScript;
    private CircleCollider2D hitbox;

    private Coroutine coroutine;
    private float timer = 0;
    private float activationTimer = 0;
    public float deactivationDelay = 0.3f;
    public float activationDelay = 2f;
    private bool isFinalTile = false;
    private Color originalColour;
    private Animator animator;

    public SpriteRenderer sr;
    // Start is called before the first frame update
    void Start()
    {
        originalColour = sr.color;
        gridScript = GameObject.FindWithTag("GridArea").GetComponent<GridAreaScript>();
        hitbox = GetComponentInChildren<CircleCollider2D>();
        hitbox.enabled = false;
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void activateTile() {
        //sr.color = new Color(1f, 0.5f, 0.5f, 0.4f);
        gameObject.SetActive(true);
        coroutine = StartCoroutine(startActivation());
    }

    IEnumerator startActivation() {

        //Start delay before it damages player
        while(activationTimer < activationDelay) {
            activationTimer += Time.deltaTime;
            yield return null;
        }
        activationTimer = 0;
        animator.SetBool("isDamaging", true);
        //coroutine = StartCoroutine(deactivateTile());
    }

    //TODO: replace timer with animation flag to deactivate tile
    void deactivateTile() {
        //while(timer < deactivationDelay) {
        //    timer += Time.deltaTime;
        //    yield return null;
        //}
        //timer = 0;
        //hitbox.enabled = false;
        gameObject.SetActive(false);
        animator.SetBool("isDamaging", false);

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

}
