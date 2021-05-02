using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealHitMeleeEnemy : MonoBehaviour
{

    public float damage;
    public float knockback;
    public GameObject parentPosition;


    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }
    public float getDamage() {
        return damage;
    }

    public float getKnockback() {
        return knockback;
    }
    public GameObject getParentPos() {
        return parentPosition;
    }



    //Deal hit melee
    //Make enemy hitbox tag
}
