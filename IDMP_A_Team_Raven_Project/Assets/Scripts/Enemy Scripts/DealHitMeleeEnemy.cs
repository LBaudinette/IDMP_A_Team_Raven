using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class DealHitMeleeEnemy : MonoBehaviour
{

    public float damage;
    public float knockback;
    public GameObject parentPosition;

    public float getDamage() {
        return damage;
    }

    public float getKnockback() {
        return knockback;
    }
    public GameObject getParentPos() {
        return parentPosition;
    }

}
