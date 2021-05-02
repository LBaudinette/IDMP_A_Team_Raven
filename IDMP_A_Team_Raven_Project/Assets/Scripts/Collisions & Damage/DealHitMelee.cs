using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealHitMelee : MonoBehaviour
{

    public float damage;
    public float knockback;
    public GameObject parentPosition;
    private PlayerMovement playerScript;

    private void Start()
    {
        playerScript = parentPosition.GetComponentInParent<PlayerMovement>();
    }


    public float getDamage()
    {
        return damage;
    }

    public float getKnockback()
    {
        return knockback;
    }

    public GameObject getParentPos()
    {
        return parentPosition;
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision != null)
        {
            if (collision.gameObject.CompareTag("Enemy"))
            {
                playerScript.addBoltToInv();
            }
        }
    }
}
