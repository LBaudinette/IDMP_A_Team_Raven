using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;
// using UnityEditor.Experimental.GraphView;

public class Enemy : MonoBehaviour {
    protected Animator animator;
    protected Rigidbody2D rb;
    protected GameObject player;
    [SerializeField] protected ParticleSystem hitPS;

    private Transform leftPlayerTarget, rightPlayerTarget;
    protected Path path;
    private Seeker seeker;
    private HitStop hitStopScript;
    private SpriteRenderer sr;
    [SerializeField] private BoxCollider2D hitBox;
    [SerializeField] protected AudioClip[] hurtSounds;
    protected AudioSource audio;


    public Sprite deathSprite, leftFlinchSprite, rightFlinchSprite;

    [SerializeField]protected float attackDelay = 0.5f;
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
    protected float maxHealth;
    [SerializeField]protected float damage = 10f;
    public LayerMask playerLayer;

    private int currentWaypoint = 0;
    [SerializeField]protected float speed = 200f;
    protected int directionFaced = (int)facingDirection.left;
    protected bool isAttacking = false;
    protected bool isMoving = false;
    protected bool isEndOfPath = false;
    public bool isDead = false;
    private Vector2 target;

    private float pathTimer = 0;
    private float pathTimerMax = 0.5f;

    private Coroutine coroutine;

    public Transform leftRaycastPoint;
    public Transform rightRaycastPoint;

    private Room roomScript;


    protected enum facingDirection {
        left,
        right
    }

    // Start is called before the first frame update
    void Start() {
        maxHealth = health;
        player = GameObject.FindWithTag("Player");
        leftPlayerTarget = player.transform.Find("Left Seek Point");
        rightPlayerTarget = player.transform.Find("Right Seek Point");
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        seeker = GetComponent<Seeker>();
        hitStopScript = GetComponent<HitStop>();
        sr = GetComponent<SpriteRenderer>();
        roomScript = this.transform.parent.GetComponentInParent<Room>();
        audio = gameObject.AddComponent<AudioSource>();
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

        //Check if we have reached the end of the path
        if (currentWaypoint >= path.vectorPath.Count) {
            isEndOfPath = true;
            return;
        }
        else
            isEndOfPath = false;

        updateTarget();

        checkAttackRange();
        if (canAttack && !isCooldown && !isFlinching)
            startMeleeAttack();
        else if (canMove)
            Move();
    }

    protected void Move() {
        // Get a vector between the next node in the path and the current position
        Vector2 direction = (path.vectorPath[currentWaypoint] - transform.position).normalized;


        if (direction.x != 0 && direction.y != 0) {
            Vector2 force = direction * speed * Time.fixedDeltaTime;
            isMoving = true;
            rb.AddForce(force);


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

        //Disable the animator so the sprite doesnt change by itself while flinching
        animator.enabled = false;
        if (directionFaced == (int)facingDirection.left)
            sr.sprite = leftFlinchSprite;
        else
            sr.sprite = rightFlinchSprite;

        while (flinchTimer < flinchDuration) {
            flinchTimer += Time.deltaTime;
            yield return null;

        }

        animator.enabled = true;

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
                if (hit.collider.tag == "Player") {
                    canAttack = true;

                }


            }

        }
        //if the enemy is facing right, fire raycast to the right
        else {
            Debug.DrawRay(rightRaycastPoint.position, Vector2.right * meleeRangeCheck, Color.green);
            hit = Physics2D.Raycast(rightRaycastPoint.position, Vector2.right, meleeRangeCheck, playerLayer);
            if (hit.collider != null) {

                if (hit.collider.tag == "Player") {
                    canAttack = true;

                }
            }

        }

    }

    public void TakeHit(Vector2 velocity, float damage) {
        //Set Rigidbody type in case player is touching enemy
        rb.bodyType = RigidbodyType2D.Dynamic;

        //Play any effects
        hitPS.Play();

        //Play random hurt noise
        int randomIndex = Random.Range(0, hurtSounds.Length);
        audio.clip = hurtSounds[randomIndex];
        audio.Play();

        //Knockback enemy
        rb.AddForce(velocity, ForceMode2D.Impulse);

        health -= damage;
        if (health <= 0) {
            //Set Rigidbody to static so player cannot move a dead enemy
            rb.bodyType = RigidbodyType2D.Static;
            animator.SetBool("isDead", true);
        }
        else {
            StartCoroutine(flinch());
        }
    }

    protected virtual void onDeath() {
        StopAllCoroutines();
        isDead = true;
        hitBox.enabled = false;
        rb.bodyType = RigidbodyType2D.Static;
        animator.SetBool("isDead", false);
        animator.enabled = false;
        sr.sprite = deathSprite;
        roomScript.enemyDied();
        this.enabled = false;
    }

    //Reenable hit boxes and reverse death animation
    public void startRevive() {
        hitBox.enabled = true;
        rb.bodyType = RigidbodyType2D.Static;
        animator.SetBool("isReviving", true);
        animator.enabled = true;
        //sr.sprite = deathSprite;
        roomScript.enemyRevived();
        this.enabled = true;
    }

    protected void endRevive() {
        isDead = false;
        rb.bodyType = RigidbodyType2D.Dynamic;
        animator.SetBool("isReviving", false);
        health = maxHealth;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.CompareTag("Hitbox")) {
            hitStopScript.freeze();
            DealHitMelee hitbox = collision.GetComponent<DealHitMelee>();
            hitbox.addBoltOnHit();
            Vector2 knockbackDir = rb.position - (Vector2)hitbox.getParentPos().transform.position;
            knockbackDir.Normalize();
            TakeHit(knockbackDir * hitbox.getKnockback(), hitbox.getDamage());

        }
    }

    private void OnCollisionEnter2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player")) {
            //Stop the rigidbody from moving if the player is pushing against it
            rb.bodyType = RigidbodyType2D.Static;
        }
        else if (collision.gameObject.CompareTag("Projectiles")) {
            if (collision.gameObject.name == "Arrow(Clone)") {
                hitStopScript.freeze();
                Arrow arrow = collision.gameObject.GetComponent<Arrow>();
                Vector2 knockbackDir = rb.position - (Vector2)arrow.getParentPos().transform.position;
                knockbackDir.Normalize();
                TakeHit(knockbackDir * arrow.getKnockback(), arrow.getDamage());
                Destroy(collision.gameObject);
            }
        }
    }

    //Allow the enemy to move again when the player is no longer pushing against it
    private void OnCollisionExit2D(Collision2D collision) {
        if (collision.gameObject.CompareTag("Player") && !isDead)
            rb.bodyType = RigidbodyType2D.Dynamic;
    }

}
