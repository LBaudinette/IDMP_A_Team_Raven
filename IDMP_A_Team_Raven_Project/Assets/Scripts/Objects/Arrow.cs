﻿using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Arrow : MonoBehaviour
{
    public Rigidbody2D rb2d;
    public float maxAliveTime;
    public float knockbackMult;
    public float damage;

    private float aliveTime;

    // Start is called before the first frame update
    void Start()
    {
        aliveTime = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        aliveTime += Time.deltaTime;
        if (aliveTime > maxAliveTime)
        {
            Destroy(gameObject);
        }
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Collisions")
        {
            // freese arrow if it collides with wall
            rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
            rb2d.freezeRotation = true;
        } else if (collision.gameObject.tag == "Enemy")
        {
            // on enemy collision, damage them and potentially apply knockback based on current rb2d force
            TakeHitScript enemy = collision.gameObject.GetComponent<TakeHitScript>();
            enemy.TakeHit(rb2d.velocity * knockbackMult, damage);
            Destroy(gameObject);

        } else if (collision.gameObject.tag == "Projectiles")
        {
            Destroy(gameObject);
        }
    }

}
