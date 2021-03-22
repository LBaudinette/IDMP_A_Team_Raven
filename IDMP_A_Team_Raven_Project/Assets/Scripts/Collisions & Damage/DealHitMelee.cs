using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealHitMelee : MonoBehaviour
{

    public float damage;
    public float knockback;
    public GameObject parentObject;

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.tag == "Enemy")
        {
            // on enemy collision, damage them and potentially apply knockback
            Vector2 diff = parentObject.transform.position - collision.gameObject.transform.position;
            float angleFloat = Mathf.Atan2(diff.y, diff.x) * Mathf.Rad2Deg;
            Quaternion angleQuat = Quaternion.Euler(0f, 0f, angleFloat);
            TakeHitScript enemy = collision.gameObject.GetComponent<TakeHitScript>();
            enemy.TakeHit(angleQuat * collision.gameObject.transform.right * knockback, damage);

        }
        else if (collision.gameObject.tag == "Projectiles")
        {
            Destroy(collision.gameObject);
        }
    }
}
