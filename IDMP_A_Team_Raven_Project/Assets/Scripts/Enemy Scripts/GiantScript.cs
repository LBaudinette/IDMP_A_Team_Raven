using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class GiantScript : MonoBehaviour
{
    public GameObject areaAttack;               //The area prefab that is used for the attack
    public float attackDistance;                /*The distance from the player that 
                                                the enemy has to be to make an attack*/
    private GameObject player;
    public float attackDelay = 1f;
    private float attackTimer = 0;
    private bool canAttack = true;
    private Coroutine coroutine;

    private Animator animator;
    public AIPath aiPath;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        Vector3 absVelocityX = aiPath.desiredVelocity.normalized;
        //Check if the enemy is moving right
        if (aiPath.desiredVelocity.x > 0f) {
            Debug.Log("velocity: " + aiPath.desiredVelocity);
            animator.SetFloat("moveX", absVelocityX.x);

            animator.SetBool("isMoving", true);
        }
        //Check if the enemy is moving left
        else if(aiPath.desiredVelocity.x < 0f) {
            Debug.Log("velocity: " + aiPath.desiredVelocity.x);
            animator.SetFloat("moveX", absVelocityX.x);

            animator.SetBool("isMoving", true);
        }
        //Enemy is not moving
        else {
            animator.SetBool("isMoving", false);
            animator.SetFloat("moveX", 0);
        }


        //check for the distance between the enemy and the player
        float distance = Vector2.Distance(gameObject.transform.position, 
            player.transform.position);
        if (distance <= attackDistance)
            //if the enemy is close enough to the player, attack
            startAttack();
    }

    private void startAttack() {
        //create a special area if enough time has passed between attacks
        if (canAttack) {
            Debug.Log("Attack");

            //Set the isAttacking boolean in the animator
            animator.SetBool("isAttacking", true);
            animator.SetBool("isMoving", false);

            canAttack = false;

        }
    }

    private void endAttack() {
        Debug.Log("Finished Attack");
        //Create the area attack
        Instantiate(areaAttack, transform.position, transform.rotation);

        animator.SetBool("isAttacking", false);
        animator.SetBool("isMoving", false);

        coroutine = StartCoroutine(startAttackCooldown());
    }


    private IEnumerator startAttackCooldown() {

        while (attackTimer < attackDelay) {
            attackTimer += Time.deltaTime;

            yield return null;
        }

        canAttack = true;
        attackTimer = 0f;
    }

    

    public void takeDamage(float damage, float force, Vector2 angle) {

    }

}
