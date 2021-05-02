using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeHitScript : MonoBehaviour
{

    public Rigidbody2D rb2d;
    private HitStop hitStopScript;

    [SerializeField] private SignalSender reducePlayerHealthSignal;

    void Start()
    {
        hitStopScript = GetComponent<HitStop>();
    }

    public void TakeHit(float damage)
    {
        //take damage
    }

    public void TakeHit(Vector2 velocity, float damage)
    {
        rb2d.AddForce(velocity);
        reducePlayerHealthSignal.Raise();
        //take damage
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("Hitbox") || collision.gameObject.CompareTag("Projectiles"))
        {
            hitStopScript.freeze();
            DealHitMelee hitbox = collision.GetComponent<DealHitMelee>();
            Vector2 knockbackDir = rb2d.position - (Vector2) hitbox.getParentPos().transform.position;
            knockbackDir.Normalize();
            TakeHit(knockbackDir * hitbox.getKnockback(), hitbox.getDamage());
        }
    }

}
