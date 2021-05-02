using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public Rigidbody2D rb2d;
    public float damage;
    public float knockbackMult;

    public float getDamage()
    {
        return damage;
    }

    public float getKnockback()
    {
        return knockbackMult;
    }

    public Vector2 getVel()
    {
        return rb2d.velocity;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        Destroy(gameObject);
    }

}
