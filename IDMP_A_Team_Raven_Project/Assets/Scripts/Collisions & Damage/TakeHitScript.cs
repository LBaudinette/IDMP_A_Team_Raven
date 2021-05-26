using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class TakeHitScript : MonoBehaviour
{

    public Rigidbody2D rb2d;
    private HitStop hitStopScript;

    [SerializeField] private SignalSender reducePlayerHealthSignal;
    [SerializeField] private FloatValue playerHealth;

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
        playerHealth.runTimeValue -= damage;
        reducePlayerHealthSignal.Raise();
        Debug.Log("player should be taking dmg");
    }

    private void OnTriggerEnter2D(Collider2D collision)
    {
        if (collision.gameObject.CompareTag("EnemyHitbox"))
        {
            hitStopScript.freeze();
            DealHitMeleeEnemy hitbox = collision.GetComponent<DealHitMeleeEnemy>();
            Vector2 knockbackDir = rb2d.position - (Vector2)hitbox.getParentPos().transform.position;
            knockbackDir.Normalize();
            TakeHit(knockbackDir * hitbox.getKnockback(), hitbox.getDamage());
        }
        else if (collision.gameObject.CompareTag("Projectiles"))
        {
            if (collision.name != "Arrow(Clone)")
            {
                hitStopScript.freeze();
                ProjectileScript hitbox = collision.GetComponent<ProjectileScript>();
                Vector2 knockbackDir = hitbox.getVel();
                knockbackDir.Normalize();
                TakeHit(knockbackDir * hitbox.getKnockback(), hitbox.getDamage());
            }
        }
        else if (collision.gameObject.CompareTag("BossHitbox"))
        {
            hitStopScript.freeze();
            BossHitbox hitbox = collision.GetComponent<BossHitbox>();
            Vector2 knockbackDir = rb2d.position - (Vector2)hitbox.getParentPos().transform.position;
            knockbackDir.Normalize();
            TakeHit(knockbackDir * hitbox.getKnockback(), hitbox.getDamage());
        }
    }

}
