using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
using UnityEditor.Experimental.GraphView;

public class Enemy : MonoBehaviour
{
    protected Animator animator;
    protected Rigidbody2D rb;
    protected GameObject player;            
    private Transform leftPlayerTarget, rightPlayerTarget;
    protected Path path;
    private Seeker seeker;

    public float attackDelay = 1f;
    private float attackTimer = 0;
    protected bool canAttack = true;

    public float nextWaypointDistance = 2f;

    
    private int currentWaypoint = 0;
    public float speed = 200f;
    bool isEndOfPath = false;
    protected int directionFaced = (int)facingDirection.left;             
    protected bool isAttacking = false;
    private Vector2 target;

    private float pathTimer = 0;
    private float pathTimerMax = 0.5f;

    protected enum facingDirection {
        left,
        right
    }

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
        leftPlayerTarget = player.transform.Find("Left Seek Point");
        rightPlayerTarget = player.transform.Find("Right Seek Point");
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        seeker = GetComponent<Seeker>();
    }

    

    protected virtual void Update() {
        
        updateTimers();
        
    }

    //Use Fixed Update due to physics being used
    void FixedUpdate()
    {
        //return if there is no path
        if (path == null)
            return;

        //Cancel any pathfinding if attacking
        if (isAttacking)
            return;

        //Check if we have reached the end of the path
        if (currentWaypoint >= path.vectorPath.Count) {
            isEndOfPath = true;
            return;
        }
        else
            isEndOfPath = false;

        updateTarget();

        //Get a vector between the next node in the path and the current position
        Vector2 direction = ((Vector2)path.vectorPath[currentWaypoint] - rb.position).normalized;
        Vector2 force = direction * speed * Time.deltaTime;

        rb.AddForce(force);
        //Debug.Log("Direction: " + direction);

        updateAnimator(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        //If the enemy is within distance to pick another node to move to, pick the next node
        if(distance < nextWaypointDistance) {
            currentWaypoint++;
        }

        
    }

    protected void OnPathComplete(Path p) {

        //if there is no error in the calculated path, make the enemy follow the path
        if (!p.error) {
            path = p;
            currentWaypoint = 0;
        }
    }

    protected void startMeleeAttack() {

        if (canAttack) {
            //Stop moving and then play the animation
            isAttacking = true;
            path = null;
            //Set the isAttacking boolean in the animator
            animator.SetBool("isAttacking", isAttacking);
            animator.SetBool("isMoving", false);

            canAttack = false;
        }
    }

    public void finishAttack() {
        Debug.Log("End Attack");
        isAttacking = false;
        canAttack = true;

        animator.SetBool("isAttacking", isAttacking);
        animator.SetBool("isMoving", false);
        updatePath();
    }

    private void updateAnimator(Vector2 force) {

        //Check if the enemy is not stationary
        if (force.x < 0f) 
            directionFaced = (int)facingDirection.left;  
         else if (force.x > 0) 
            directionFaced = (int)facingDirection.right;

        //Enemy is not moving
        //else {
        //    animator.SetBool("isMoving", false);
        //}

        animator.SetFloat("moveX", force.x);
        animator.SetBool("isMoving", true);

    }

    private void updateTarget() {
        float distanceToLeft = Vector2.Distance(transform.position, leftPlayerTarget.position);
        float distanceToRight = Vector2.Distance(transform.position, rightPlayerTarget.position);

        if (distanceToLeft < distanceToRight)
            target = leftPlayerTarget.position;
        else
            target = rightPlayerTarget.position;
    }
    protected void updatePath() { 
        //if the path is finished , calculate the new one
        if (seeker.IsDone())
            seeker.StartPath(rb.position, target, OnPathComplete);
    }

    protected void updateTimers() {
        //Update the pathfinding for the enemy at regular intervals
        if (pathTimer < pathTimerMax) {
            pathTimer += Time.deltaTime;
        }
        else {
            pathTimer = 0;
            updatePath();
        }

        //if not attacking, start the timer
        if (!isAttacking) {
            //Update the attack timer
            if (attackTimer < attackDelay) {
                attackTimer += Time.deltaTime;
            }
            else {
                attackTimer = 0;
                canAttack = true;
            }
        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        startMeleeAttack();
    }
}
