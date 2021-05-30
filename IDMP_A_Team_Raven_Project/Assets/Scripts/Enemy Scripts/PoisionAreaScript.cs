using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PoisionAreaScript : SpecialArea
{
    public float maxScaleSize = 5f;             //The maximum size of the area
    public float maxScaleTime = 5f;             //The time it will take to reach the maximum size
    public float maxLingerTime = 5f;
    public float diminishTime = 5f;

    private Coroutine coroutine;
    private CircleCollider2D col;
    private bool canDamage = true;
    private float damageTimer = 0f;
    public float damageCD;

    public float playerDamage = 0f;

    // Start is called before the first frame update
    void Start()
    {
        coroutine = StartCoroutine(startExpansion(maxScaleSize, maxScaleTime, 
            maxLingerTime, coroutine, diminishTime));
        col = GetComponentInChildren<CircleCollider2D>();
    }

    private void Update() {

        if (damageTimer < damageCD && !col.enabled)
            damageTimer += Time.deltaTime;
        else {
            col.enabled = true;
            damageTimer = 0f;
        }

    }

    private void OnTriggerEnter2D(Collider2D collision) {
        col.enabled = false;
    }
    private void OnTriggerStay(Collider other) {
        col.enabled = false;
    }

    //private void OnTriggerExit2D(Collider2D collision) {
    //    if (collision.gameObject.tag == "Player") {
    //        GameObject player = collision.gameObject;
    //    }
    //}
}
