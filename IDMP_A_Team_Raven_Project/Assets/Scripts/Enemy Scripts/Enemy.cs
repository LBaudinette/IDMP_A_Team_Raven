using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
// using UnityEditor.Experimental.GraphView;

public class Enemy : MonoBehaviour {
    protected Animator animator;
    protected Rigidbody2D rb;
    protected GameObject player;
    private Transform leftPlayerTarget, rightPlayerTarget;
    protected Path path;
    private Seeker seeker;
    private HitStop hitStopScript;

    public float attackDelay = 0.5f;
    private float attackTimer = 0;
    protected bool canAttack = false;       //flags whether the player is in range for an attack
    protected bool canMove = true;
    protected bool isFlinching = false;
    protected bool isCooldown = false;
    protected float meleeRangeCheck = 1.5f;

    protected float flinchDuration = 1f;
    protected float flinchTimer = 0f;

    public float nextWaypointDistance = 2f;
    public float health = 100f;
    public float damage = 10f;
    public LayerMask playerLayer;

    private int currentWaypoint = 0;
    public float speed = 200f;
    protected int directionFaced = (int)facingDirection.left;
    protected bool isAttacking = false;
    protected bool isMoving = false;
    private Vector2 target;

    private float pathTimer = 0;
    private float pathTimerMax = 0.5f;

    private Coroutine coroutine;

    public Transform leftRaycastPoint;
    public Transform rightRaycastPoint;

    protected enum facingDirection {
        left,
        right
    }

    // Start is called before the first frame update
    void Start() {
        player = GameObject.FindWithTag("Player");
        leftPlayerTarget = player.transform.Find("Left Seek Point");
        rightPlayerTarget = player.transform.Find("Right Seek Point");
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        seeker = GetComponent<Seeker>();
        hitStopScript = GetComponent<HitStop>();
    }



    protected virtual void Update() {

        updateTimers();

    }

    //Use Fixed Update due to physics being used
    void FixedUpdate() {
        //return if there is no path
        if (path == null)
            return;

        //Cancel any pathfinding if attacking or dead
        if (isAttacking && health != 0)
            return;

        //Debug.Log("Cooldown: " + isCooldown);
        //Debug.Log("Can Attack: " + canAttack);

        checkAttackRange();
        if (canAttack && !isCooldown && !isFlinching)
            startMeleeAttack();
        else if (canMove)
            Move();


        updateTarget();
        //Debug.Log("Can Attack: " + canAttack);
        //Debug.Log("Is Cooldown: " + isCooldown);

    }

    protected void Move() {
        // Get a vector between the next node in the path and the current position
        Vector2 direction = (path.vectorPath[currentWaypoint] - transform.position).normalized;


        if (direction.x != 0 && direction.y != 0) {
            //Debug.Log(direction);
            Vector2 force = direction * speed * Time.fixedDeltaTime;
            isMoving = true;
            //Debug.Log("Force: " + force);
            rb.AddForce(force);
            //transform.Translate(force);
            //Debug.Log("Direction: " + direction);

            updateAnimator(force);
        }




        float distance = Vector2.Distance(rb.position, path.vectorPath[currentWaypoint]);
        //If the enemy is within distance to pick another node to move to, pick the next node
        if (distance < nextWaypointDistance) {
            currentWaypoint++;
        }
    }

    protected IEnumerator flinch() {
        isFlinching = true;
        canMove = false;
        isCooldown = true;

        while (flinchTimer < flinchDuration) {
            flinchTimer += Time.deltaTime;
            yield return null;

        }
        flinchTimer = 0f;
        canMove = true;
        isFlinching = false;

    }

    protected void OnPathComplete(Path p) {

        //if there is no error in the calculated path, make the enemy follow the path
        if (!p.error) {
            path = p;
            currentWaypoint = 0;
        }
    }

    protected void startMeleeAttack() {

        //Stop moving and then play the animation
        isAttacking = true;
        canMove = false;
        isMoving = false;
        path = null;

        //Stop the enemy from moving
        rb.velocity = new Vector2(0, 0);

        //Set the isAttacking boolean in the animator
        animator.SetBool("isAttacking", isAttacking);
        animator.SetBool("isMoving", isMoving);

        Debug.Log("Starting Attack");
        canAttack = false;

    }

    public void finishAttack() {
        Debug.Log("End Attack");
        isAttacking = false;
        isMoving = true;
        canMove = true;
        isCooldown = true;

        animator.SetBool("isAttacking", isAttacking);
        animator.SetBool("isMoving", false);
        updatePath();
    }

    private void updateAnimator(Vector2 force) {
        //Update Direction
        float direction = GameObject.FindWithTag("Player").transform.position.x - transform.position.x;
        //Check if the enemy is not stationary
        if (direction < 0) {
            directionFaced = (int)facingDirection.left;
            animator.SetFloat("moveX", -1);

        }
        else if (direction > 0) {
            //else if (force.x > 0) {
            directionFaced = (int)facingDirection.right;
            animator.SetFloat("moveX", 1);

        }
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

        //if on cooldown, start the timer
        if (isCooldown) {
            //Update the attack timer
            if (attackTimer < attackDelay) {
                attackTimer += Time.deltaTime;
            }
            else {
                attackTimer = 0;
                isCooldown = false;
            }
        }
    }

    protected void checkAttackRange() {

        RaycastHit2D hit;


        //if the enemy is facing left, fire raycast to the left
        if (directionFaced == (int)facingDirection.left) {
            Debug.DrawRay(leftRaycastPoint.position, Vector2.left * meleeRangeCheck, Color.green);
            hit = Physics2D.Raycast(leftRaycastPoint.position, Vector2.left, meleeRangeCheck, playerLayer);
            if (hit.collider != null) {
                Debug.Log("TAG: " + hit.collider.tag);
                if (hit.collider.tag == "Player") {
                    Debug.Log("HIT");
                    canAttack = true;

                }


            }

        }
        //if the enemy is facing right, fire raycast to the right
        else {
            Debug.DrawRay(rightRaycastPoint.position, Vector2.right * meleeRangeCheck, Color.green);
            hit = Physics2D.Raycast(rightRaycastPoint.position, Vector2.right, meleeRangeCheck, playerLayer);
            if (hit.collider != null) {
                Debug.Log("TAG: " + hit.collider.tag);

                if (hit.collider.tag == "Player") {
                    Debug.Log("HIT");
                    canAttack = true;

                }
            }

        }

    }

    public void TakeHit(Vector2 velocity, float damage) {
        StartCoroutine(flinch());
        Debug.Log("HIT TAKEN");
        rb.bodyType = RigidbodyType2D.Dynamic;

        rb.AddForce(velocity, ForceMode2D.Impulse);

        if (health <= 0) {
            rb.bodyType = RigidbodyType2D.Static;
            animator.SetBool("isDead", true);
        }
        health -= damage;
    }

    protected virtual void onDeath() {
        //StopCoroutine(coroutine);
        //animator.speed = 0f;
        rb.bodyType = RigidbodyType2D.Static;
        //Destroy(this);
        Destroy(gameObject);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Hitbox")) {
            hitStopScript.freeze();
            DealHitMelee hitbox = collision.GetComponent<DealHitMelee>();
            Vector2 knockbackDir = rb.position - (Vector2)hitbox.getParentPos().transform.position;
            knockbackDir.Normalize();
            TakeHit(knockbackDir * hitbox.getKnockback(), hitbox.getDamage());
        }
    }

    private void OnCollisionEnter2D(Collision collision)
    {
        if (collision.gameObject.CompareTag("Projectiles"))
        {
            hitStopScript.freeze();
            Arrow arrow = collision.gameObject.GetComponent<Arrow>();
            Vector2 knockbackDir = rb.position - (Vector2)arrow.getParentPos().transform.position;
            knockbackDir.Normalize();
            TakeHit(knockbackDir * arrow.getKnockback(), arrow.getDamage());
        }
    }

    //Stop the rigidbody from moving if the player is pushing against it
    private void OnCollisionEnter2D(Collision2D collision) {
        Debug.Log("COLLIDE");
        if (collision.gameObject.tag == "Player")
            rb.bodyType = RigidbodyType2D.Static;
    }

    //Allow the enemy to move again when the player is no longer pushing against it
    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.tag == "Player")
            rb.bodyType = RigidbodyType2D.Dynamic;
    }

}
