using System.Collections;
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
        if (collision.gameObject.CompareTag ("Collisions"))
        {
            // freese arrow if it collides with wall
            rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
            rb2d.freezeRotation = true;
        } else if (collision.gameObject.CompareTag("Enemy") || collision.gameObject.CompareTag("Projectiles"))
        {
            Destroy(gameObject);
        }
    }

    public float getDamage()
    {
        return damage;
    }

    public float getKnockback()
    {
        return knockbackMult;
    }

    public GameObject getParentPos()
    {
        return this.gameObject;
    }


}
