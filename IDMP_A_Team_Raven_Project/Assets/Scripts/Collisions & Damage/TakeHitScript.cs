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

}
