using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class ProjectileScript : MonoBehaviour
{
    public Rigidbody2D rb2d;
    public float damage;
    public float knockbackMult;
    private HitStop hitStopScript;

    // Start is called before the first frame update
    void Start()
    {
        hitStopScript = GetComponent<HitStop>();
    }

    // Update is called once per frame
    void Update()
    {

    }

    private void OnCollisionEnter2D(Collision2D collision) {
        Destroy(gameObject);
        if (collision.gameObject.tag == "Player")
        {
            hitStopScript.freeze();
            TakeHitScript player = collision.gameObject.GetComponent<TakeHitScript>();
            player.TakeHit(rb2d.velocity * knockbackMult, damage);
        } else if (collision.gameObject.tag == "Collisions" || collision.gameObject.tag == "Projectiles")
        {
            Destroy(gameObject);
        }
        //TODO: Deal damage to player
    }

}
