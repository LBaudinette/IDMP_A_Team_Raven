using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DamagingTileScript : MonoBehaviour
{
    public int damage = 10;

    private Coroutine coroutine;
    private float timer = 0;
    private float activationTimer = 0;
    public float deactivationDelay = 0.3f;
    public float activationDelay = 2f;
    private Color originalColour;

    public SpriteRenderer sr;
    // Start is called before the first frame update
    void Start()
    {
        originalColour = sr.color;
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    public void activateTile() {
        sr.color = new Color(1f, 0.5f, 0.5f, 0.4f);
        gameObject.SetActive(true);
        coroutine = StartCoroutine(startActivation());
        
    }

    IEnumerator startActivation() {
        //Start delay before it damages player
        while(activationTimer < activationDelay) {
            Debug.Log(activationTimer);
            activationTimer += Time.deltaTime;
            yield return null;
        }
        activationTimer = 0;
        sr.color = new Color(1f,1f,1f,1f);

        coroutine = StartCoroutine(deactivateTile());
    }

    //TODO: replace timer with animation flag to deactivate tile
    IEnumerator deactivateTile() {
        while(timer < deactivationDelay) {
            timer += Time.deltaTime;
            yield return null;
        }
        timer = 0;

        gameObject.SetActive(false);
    }

}
