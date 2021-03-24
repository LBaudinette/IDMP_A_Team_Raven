using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemy : MonoBehaviour
{
    protected Animator animator;
    protected bool canAttack;
    protected Rigidbody2D rb;

    private Transform leftPlayer, rightPlayer;
    public float nextWaypointDistance = 3f;

    Path path;
    Seeker seeker;
    int currentWaypoint = 0;
    public float speed = 200f;
    bool isEndOfPath = false;

    Vector2 target;

    // Start is called before the first frame update
    void Start()
    {
        leftPlayer = GameObject.FindWithTag("Player").transform.Find("Left Seek Point");
        rightPlayer = GameObject.FindWithTag("Player").transform.Find("Right Seek Point");
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        seeker = GetComponent<Seeker>();

        InvokeRepeating("updatePath", 0f, 0.5f);

    }

    void updatePath() {

        //if the path is finished calculating, calculate the new one
        if(seeker.IsDone())
            seeker.StartPath(rb.position, target, OnPathComplete);
    }

    //Use Fixed Update due to physics being used
    void FixedUpdate()
    {
        if (path == null)
            return;

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
        Debug.Log("Direction: " + direction);
        updateAnimator(force);

        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);

        //If the enemy is within distance to pick another node to move to, pick the next node
        if(distance < nextWaypointDistance) {
            currentWaypoint++;
        }

        
    }

    protected void OnPathComplete(Path p) {

        //if there is no error in the calculated path, make a new path
        if (!p.error) {
            path = p;
            currentWaypoint = 0;
        }
    }

    protected virtual void startAttack() {

    }

    private void updateAnimator(Vector2 force) {

        //Check if the enemy is moving right
        if (force.x != 0f) {
            //Debug.Log("velocity: " + rb.velocity.x);
            animator.SetFloat("moveX", force.x);

            animator.SetBool("isMoving", true);
        }
        //Enemy is not moving
        else {
            animator.SetBool("isMoving", false);
        }
    }

    private void updateTarget() {
        float distanceToLeft = Vector2.Distance(transform.position, leftPlayer.position);
        float distanceToRight = Vector2.Distance(transform.position, rightPlayer.position);

        if (distanceToLeft < distanceToRight)
            target = leftPlayer.position;
        else
            target = rightPlayer.position;
    }
}
