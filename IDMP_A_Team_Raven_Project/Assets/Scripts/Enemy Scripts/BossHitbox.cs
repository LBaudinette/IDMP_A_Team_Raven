using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class BossHitbox : MonoBehaviour
{
    public float damage;
    public float knockback;

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
        return gameObject;
    }
}
