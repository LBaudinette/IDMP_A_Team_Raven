﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : MonoBehaviour
{

    private Vector2 playerPos;
    private float firingDelay;          //The delay between firing projectiles
    private float timer;                //Keeps track of timer between shots
    public GameObject fireball;         //Prefab of projectile
    public float projForce;
    public Transform firePoint;
    public Transform rotationPoint;

    // Start is called before the first frame update
    void Start()
    {
        firingDelay = 2f;
        timer = 0f;
        playerPos = GameObject.FindWithTag("Player").transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        playerPos = GameObject.FindWithTag("Player").transform.position;
        //Check to see if the Enemy can fire a projectile
        if (timer <= 0) {
            fireProjectile();
            timer = firingDelay;
        }
        else
            timer -= Time.deltaTime;
    }

    void fireProjectile() {
        //Find the vector between the player and the object firing the projectile
        Vector2 vectorDifference = playerPos - (Vector2)transform.position;

        //Find the angle between the two objects using the vector we just calculated in degrees
        float angleFloat = Mathf.Atan2(vectorDifference.y, vectorDifference.x) * Mathf.Rad2Deg;
        
        // update rotation of fire point
        rotationPoint.rotation = Quaternion.Euler(0f, 0f, angleFloat);

        //Create projectile
        GameObject proj = Instantiate(fireball, firePoint.position, rotationPoint.rotation);
        proj.GetComponent<Rigidbody2D>().AddForce(firePoint.right * projForce, ForceMode2D.Impulse);
        Debug.Log("fired projectile");
    }
}
