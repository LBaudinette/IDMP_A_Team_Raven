using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class FrostAreaScript : SpecialArea
{
    public float maxScaleSize = 5f;             //The maximum size of the area
    public float maxScaleTime = 5f;             //The time it will take to reach the maximum size
    public float maxLingerTime = 5f;
    public float diminishTime = 5f;

    private Coroutine coroutine;
    public float playerDrag = 50f;              //The factor to increase the players drag

    // Start is called before the first frame update
    void Start()
    {
        coroutine = StartCoroutine(startExpansion(maxScaleSize,maxScaleTime,
            maxLingerTime, coroutine, diminishTime));
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    //Slow player when they enter to area
    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            GameObject player = collision.gameObject;
            player.GetComponent<Rigidbody2D>().drag = playerDrag;
        }
    }

    //Stop slowing player when they exit the area
    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            GameObject player = collision.gameObject;
            player.GetComponent<Rigidbody2D>().drag = 0f;
        }
    }
}
