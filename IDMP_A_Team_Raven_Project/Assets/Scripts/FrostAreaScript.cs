using System.Collections;
using System.Collections.Generic;
using System;
using UnityEngine;

public class FrostAreaScript : MonoBehaviour
{
    public float maxScaleSize;
    public float maxScaleTime;
    private float scaleTimer = 0;
    private Coroutine coroutine;

    public float maxLingerTime = 5f;
    private bool isLingering = false;
    private float lingerTime = 0f;

    public float playerDrag = 50f;              //The factor to increase the players drag
    // Start is called before the first frame update
    void Start()
    {
        coroutine = StartCoroutine(startExpansion());
    }

    // Update is called once per frame
    void Update() {

        //Allow the area to linger before disappearing
        if (isLingering) {
            if (lingerTime < maxLingerTime) 
                lingerTime += Time.deltaTime;
            else
                //Can allow to fade away
                Destroy(gameObject);
            
        }
    }

    IEnumerator startExpansion() {
        //Get the original scale and set the target scale as a vector
        Vector3 currentScale = transform.localScale;
        Vector3 targetScale = new Vector3(maxScaleSize, maxScaleSize, 0f);

        //Keep increasing the size of the area using lerp
        while(scaleTimer < maxScaleTime) {
            transform.localScale = Vector3.Lerp(currentScale, targetScale, scaleTimer / maxScaleSize);

            scaleTimer += Time.deltaTime;

            yield return null;
        }

        isLingering = true;
    }

    private void OnTriggerEnter2D(Collider2D collision) {

        if (collision.gameObject.tag == "Player") {
            GameObject player = collision.gameObject;
            player.GetComponent<Rigidbody2D>().drag = playerDrag;
        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            GameObject player = collision.gameObject;
            player.GetComponent<Rigidbody2D>().drag = 0f;
        }
    }


}
