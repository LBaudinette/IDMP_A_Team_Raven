using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeHitScript : MonoBehaviour
{

    public Rigidbody2D rb2d;

    public void TakeHit(float damage)
    {
        //take damage
    }

    public void TakeHit(Vector2 velocity, float damage)
    {
        rb2d.AddForce(velocity);
        //take damage
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.tag == "Hitbox")
        {
            DealHitMelee hitbox = collision.GetComponent<DealHitMelee>();
            Vector2 knockbackDir = rb2d.position - (Vector2) hitbox.getParentPos().transform.position;
            knockbackDir.Normalize();
            TakeHit(knockbackDir * hitbox.getKnockback(), hitbox.getDamage());
        }
    }

}
