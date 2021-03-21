using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisionAreaScript : SpecialArea
{
    public float maxScaleSize = 5f;             //The maximum size of the area
    public float maxScaleTime = 5f;             //The time it will take to reach the maximum size
    private Coroutine coroutine;

    public float maxLingerTime = 5f;

    public float playerDamage = 0f;

    // Start is called before the first frame update
    void Start()
    {
        coroutine = StartCoroutine(startExpansion(maxScaleSize, maxScaleTime, maxLingerTime, coroutine));
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            GameObject player = collision.gameObject;
            //TODO: Deal with player damage

        }
    }

    private void OnTriggerExit2D(Collider2D collision) {
        if (collision.gameObject.tag == "Player") {
            GameObject player = collision.gameObject;
            //TODO: Deal with player damage
        }
    }
}
