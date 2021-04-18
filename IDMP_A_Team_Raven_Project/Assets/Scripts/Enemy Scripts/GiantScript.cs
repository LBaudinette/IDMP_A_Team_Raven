using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class GiantScript : Enemy
{
    public GameObject areaAttack;               //The area prefab that is used for the attack
    public float attackDistance = 3f;
    public Transform leftAttackPoint;
    public Transform rightAttackPoint;

    private Coroutine coroutine;

   // Update is called once per frame
   //protected override void Update() {


   //     //check for the distance between the enemy and the player
   //     float distance = Vector2.Distance(gameObject.transform.position,
   //         player.transform.position);
   //     if (distance <= attackDistance)
   //         //if the enemy is close enough to the player, attack
   //         startAreaAttack();
   //     updateTimers();
   // }

    //private void startAreaAttack() {
    //    //create a special area if enough time has passed between attacks
    //    if (canAttack) {
    //        Debug.Log("ATTACK");
    //        isAttacking = true;
    //        path = null;

    //        //Set the isAttacking boolean in the animator
    //        animator.SetBool("isAttacking", true);
    //        //animator.SetBool("isMoving", false);

    //        canAttack = false;

    //    }
    //}

    void finishAreaAttack() {
        Transform spawnPos = directionFaced == (int)facingDirection.left ? leftAttackPoint : rightAttackPoint; 
        Instantiate(areaAttack, spawnPos.position, transform.rotation);

        isAttacking = false;
        isMoving = true;
        isCooldown = true;

        animator.SetBool("isAttacking", isAttacking);
        animator.SetBool("isMoving", true);
        updatePath();
    }
    //private void OnCollisionEnter2D(Collision2D collision) {
    //    if(!isAttacking && collision.transform.gameObject.tag == "Player")
    //        startAreaAttack();
    //}

    //private void endAttack() {
    //    Debug.Log("Finished Attack");
    //    //Create the area attack
    //    Instantiate(areaAttack, transform.position, transform.rotation);

    //    animator.SetBool("isAttacking", false);
    //    animator.SetBool("isMoving", false);

    //    coroutine = StartCoroutine(startAttackCooldown());
    //}


    //private IEnumerator startAttackCooldown() {

    //    while (attackTimer < attackDelay) {
    //        attackTimer += Time.deltaTime;

    //        yield return null;
    //    }

    //    canAttack = true;
    //    attackTimer = 0f;
    //}



    

}
