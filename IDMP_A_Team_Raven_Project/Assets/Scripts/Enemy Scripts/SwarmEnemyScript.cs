using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class SwarmEnemyScript : Enemy
{

    // Start is called before the first frame update
    void Start()
    {
        
    }

    // Update is called once per frame
    void Update()
    {
        
    }

    private void startAttack() {

        if (canAttack) {
            Debug.Log("Attack");

            //Set the isAttacking boolean in the animator
            base.animator.SetBool("isAttacking", true);
            animator.SetBool("isMoving", false);

            canAttack = false;

        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if(collision.gameObject.tag == "Player") {
            startAttack();
        }
    }
}
