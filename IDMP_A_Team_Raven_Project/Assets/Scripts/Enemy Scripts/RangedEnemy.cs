using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : MonoBehaviour {

    private Vector2 playerPos;
    protected float attackDelay = 2f;               //The delay between firing projectiles
    protected float attackTimer = 0f;               //Keeps track of timer between shots
    public GameObject fireball;                     //Prefab of projectile
    public float projForce;
    public Transform firePoint;
    public Transform rotationPoint;
    public float teleTriggerDistance = 1;           //How close the player must be to start teleporting
    protected HitStop hitStopScript;

    public float health = 100f;
    protected float maxHealth;

    protected float teleportTimer = 0;
    protected float teleportCDTimer = 0;            //Keeps track of time between teleports
    public float teleportDelay = 3;                 //How long it takes to teleport
    public float teleportDistance = 3f;
    public float teleportCooldown = 4f;
    
    protected bool isTeleporting = false;
    protected bool canTeleport = true;
    protected bool isAttacking = false;
    public bool isDead = false;

    //Raycast points used for teleporting
    public Transform leftRaycastPoint, topLeftRaycastPoint, topRaycastPoint,
        topRightRaycastPoint, rightRaycastPoint, bottomRightRaycastPoint,botRaycastPoint,
        bottomLeftRaycastPoint;
    public LayerMask layerDetection;

    protected Vector2 originalPosition;             //Used to keep track of position before teleports


    protected Animator animator;
    protected Rigidbody2D rb;
    protected Coroutine coroutine;
    [SerializeField] protected ParticleSystem teleportPS;
    [SerializeField] protected ParticleSystem hitPS;
    protected AfterImageScript afterImageScript;
    [SerializeField] protected AudioClip[] hurtSounds;
    protected AudioSource audio;

    private Room roomScript;

    //Struct that stores the ray and hit for a raycast
    private struct Raycast {

        public Ray2D ray;
        public RaycastHit2D raycastHit;

        public Raycast(Ray2D Ray, RaycastHit2D RaycastHit2D) {
            ray = Ray;
            raycastHit = RaycastHit2D;
        }
    }

    // Start is called before the first frame update
    void Start() {
        maxHealth = health;
        playerPos = GameObject.FindWithTag("Player").transform.position;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        afterImageScript = GetComponent<AfterImageScript>();
        originalPosition = gameObject.transform.position;
        roomScript = this.transform.parent.GetComponentInParent<Room>();
        hitStopScript = GetComponent<HitStop>();
        audio = gameObject.AddComponent<AudioSource>();
        audio.volume = 0.8f;

    }

    // Update is called once per frame
    void Update() {
        

        //Update the player position every frame
        playerPos = GameObject.FindWithTag("Player").transform.position;


        //Update Direction
        float direction = playerPos.x - transform.position.x;
        if (direction < 0)
            animator.SetFloat("playerDirection", -1);
        else if (direction > 0)
            animator.SetFloat("playerDirection", 1);


        //Check to see if the Enemy can fire a projectile
        if (attackTimer < attackDelay)
            attackTimer += Time.deltaTime;
        else if(!isTeleporting){
            animator.SetBool("isAttacking", true);
            isAttacking = true;
        }

        if (teleportCDTimer < teleportCooldown && !isTeleporting)
            teleportCDTimer += Time.deltaTime;
        else {
            canTeleport = true;
            teleportCDTimer = 0f;
        }

        //Check if the player is within distance to teleport
        if (Vector2.Distance(transform.position, playerPos) < teleTriggerDistance
            && canTeleport && !isTeleporting) {
            canTeleport = false;
            coroutine = StartCoroutine(startTeleport());

        }
    }

    void fireProjectile() {
        //Find the vector between the player and the object firing the projectile
        Vector2 vectorDifference = playerPos - (Vector2)transform.position;

        //Find the angle between the two objects using the vector we just calculated in degrees
        float angleFloat = Mathf.Atan2(vectorDifference.y, vectorDifference.x) * Mathf.Rad2Deg;

        // update rotation of fire point
        rotationPoint.rotation = Quaternion.Euler(0f, 0f, angleFloat);

        //Create projectile
        GameObject proj = Instantiate(fireball, firePoint.position, rotationPoint.rotation);
        proj.GetComponent<Rigidbody2D>().AddForce(firePoint.right * projForce, ForceMode2D.Impulse);
    }

    void finishAttack() {
        isAttacking = false;
        animator.SetBool("isAttacking", false);
        attackTimer = 0;
    }


    protected IEnumerator startTeleport() {
        teleportPS.Play();
        isTeleporting = true;
        canTeleport = false;
        animator.SetBool("isTeleporting", true);
        Debug.Log("Starting Teleport");

        //Start the teleport timer
        while (teleportTimer < teleportDelay) {
            //Debug.Log("TIMER: " + teleportTimer);
            teleportTimer += Time.deltaTime;

            yield return null;
        }

        teleportPS.Stop();
        teleport();
    }

    void teleport() {


        var hitRaycasts = new List<Raycast>();
        var emptyRaycasts = new List<Raycast>();
        var rays = new List<Raycast>();

        //Teleport
        //Fire 8 raycasts to check for available teleports
        RaycastHit2D leftHit, topLeftHit,topHit,topRightHit,rightHit, botRightHit,
            botHit,botLeftHit;

        Ray2D leftRay = new Ray2D(leftRaycastPoint.position, Vector2.left);
        Ray2D topLeftRay = new Ray2D(topLeftRaycastPoint.position, new Vector2(-1, 1));
        Ray2D topRay = new Ray2D(topRaycastPoint.position, Vector2.up);
        Ray2D topRightRay = new Ray2D(topRightRaycastPoint.position, new Vector2(1, 1));
        Ray2D rightRay = new Ray2D(rightRaycastPoint.position, Vector2.right);
        Ray2D botRightRay = new Ray2D(bottomRightRaycastPoint.position, new Vector2(1, -1));
        Ray2D botRay = new Ray2D(botRaycastPoint.position, Vector2.down);
        Ray2D botLeftRay = new Ray2D(bottomLeftRaycastPoint.position, new Vector2(-1, -1));


        leftHit = Physics2D.Raycast(leftRay.origin, leftRay.direction, teleTriggerDistance, layerDetection);
        topLeftHit = Physics2D.Raycast(topLeftRay.origin, topLeftRay.direction, teleTriggerDistance, layerDetection);
        topHit = Physics2D.Raycast(topRay.origin, topRay.direction, teleportDistance, layerDetection);
        topRightHit = Physics2D.Raycast(topRightRay.origin, topRightRay.direction, teleTriggerDistance, layerDetection);
        rightHit = Physics2D.Raycast(rightRay.origin, rightRay.direction, teleportDistance, layerDetection);
        botRightHit = Physics2D.Raycast(botRightRay.origin, botRightRay.direction, teleTriggerDistance, layerDetection);
        botHit = Physics2D.Raycast(botRay.origin, botRay.direction, teleportDistance, layerDetection);
        botLeftHit = Physics2D.Raycast(botLeftRay.origin, botLeftRay.direction, teleTriggerDistance, layerDetection);



        Raycast leftRaycast = new Raycast(leftRay, leftHit);
        Raycast topLeftRaycast = new Raycast(topLeftRay, topLeftHit);
        Raycast topRaycast = new Raycast(topRay, topHit);
        Raycast topRightRaycast = new Raycast(topRightRay, topRightHit);
        Raycast rightRaycast = new Raycast(rightRay, rightHit);
        Raycast botRightRaycast = new Raycast(botRightRay, botRightHit);
        Raycast botRaycast = new Raycast(botRay, botHit);
        Raycast botLeftRaycast = new Raycast(botLeftRay, botLeftHit);



        rays.Add(leftRaycast);
        rays.Add(topLeftRaycast);
        rays.Add(topRaycast);
        rays.Add(topRightRaycast);
        rays.Add(rightRaycast);
        rays.Add(botRightRaycast);
        rays.Add(botRaycast);
        rays.Add(botLeftRaycast);


        //Assign longest hit to an existing hit so we can compare to others
        RaycastHit2D longestHit = leftHit;

        //Check if each one hit something
        foreach (Raycast raycast in rays) {
            if (raycast.raycastHit.collider != null)
                hitRaycasts.Add(raycast);
            else
                emptyRaycasts.Add(raycast);

        }

        //Debug.Log("NUMBER OF COLLISIONS: " + hitRaycasts.Count);
        //Debug.Log("NUMBER OF EMPTY SPACES: " + emptyRaycasts.Count);

        Raycast longestRaycast;

        //if no walls were hit, then teleport into an open space
        if (emptyRaycasts.Count != 0) {
            //assign the first element as the longest
            longestRaycast = emptyRaycasts[0];

            //pick a random direction
            int randomIndex = Random.Range(0, emptyRaycasts.Count - 1);

            //Teleport to the open space
            //Debug.Log("TELEPORT INTO OPEN SPACE");
            originalPosition = transform.position;
            transform.Translate(emptyRaycasts[randomIndex].ray.direction * teleportDistance);
            Debug.Log("ORIGINAL POS: " + originalPosition);
            Debug.Log("NEW POS: " + transform.position);
            afterImageScript.createAfterImageTrail(originalPosition, transform.position, GetComponent<SpriteRenderer>().sprite);
        }
        //if there are no empty spaces, teleport next to a wall
        else {
            longestRaycast = hitRaycasts[0];

            foreach (Raycast raycast in hitRaycasts) {
                if (raycast.raycastHit.distance > longestRaycast.raycastHit.distance)
                    longestRaycast = raycast;
            }

            //Debug.Log("TELEPORT NEAR WALL");

            //Teleport to the walls position
            //transform.Translate(longestRaycast.ray.direction * teleportDistance);
            rb.MovePosition((Vector2)transform.position + (longestRaycast.ray.direction * teleportDistance));

        }
        isTeleporting = false;
        canTeleport = false;
        animator.SetBool("isTeleporting", false);
        teleportTimer = 0;
    }

    

    protected virtual void onDeath() {
        StopAllCoroutines();
        isDead = true;
        gameObject.SetActive(false);
        roomScript.enemyDied();
        //GetComponent<BoxCollider2D>().enabled = false;
        //animator.speed = 0f;
        //Destroy(this);
    }

    protected virtual void TakeHit(Vector2 velocity, float damage) {
        hitPS.Play();

        //Play random hurt noise
        int randomIndex = Random.Range(0, hurtSounds.Length);
        audio.clip = hurtSounds[randomIndex];
        audio.Play();

        rb.AddForce(velocity * 5);
        health -= damage;
        if (health <= 0)
            animator.SetBool("isDead", true);
        
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Hitbox") {
            hitStopScript.freeze();
            DealHitMelee hitbox = collision.GetComponent<DealHitMelee>();
            hitbox.addBoltOnHit();
            Vector2 knockbackDir = rb.position - (Vector2)hitbox.getParentPos().transform.position;
            knockbackDir.Normalize();
            TakeHit(knockbackDir * hitbox.getKnockback(), hitbox.getDamage());
        }
    }

    private void drawDebugRaycasts() {
        Ray2D leftRay = new Ray2D(leftRaycastPoint.position, Vector2.left);
        Ray2D topLeftRay = new Ray2D(topLeftRaycastPoint.position, new Vector2(-1, 1));
        Ray2D topRay = new Ray2D(topRaycastPoint.position, Vector2.up);
        Ray2D topRightRay = new Ray2D(topRightRaycastPoint.position, new Vector2(1, 1));
        Ray2D rightRay = new Ray2D(rightRaycastPoint.position, Vector2.right);
        Ray2D botRightRay = new Ray2D(bottomRightRaycastPoint.position, new Vector2(1, -1));
        Ray2D botRay = new Ray2D(botRaycastPoint.position, Vector2.down);
        Ray2D botLeftRay = new Ray2D(bottomLeftRaycastPoint.position, new Vector2(-1, -1));

        Debug.DrawRay(leftRay.origin, leftRay.direction * teleTriggerDistance, Color.green);
        Debug.DrawRay(topLeftRay.origin, topLeftRay.direction * teleTriggerDistance, Color.green);
        Debug.DrawRay(topRay.origin, topRay.direction * teleportDistance, Color.green);
        Debug.DrawRay(topRightRay.origin, topRightRay.direction * teleportDistance, Color.green);
        Debug.DrawRay(rightRay.origin, rightRay.direction * teleportDistance, Color.green);
        Debug.DrawRay(botRightRay.origin, botRightRay.direction * teleportDistance, Color.green);
        Debug.DrawRay(botRay.origin, botRay.direction * teleportDistance, Color.green);
        Debug.DrawRay(botLeftRay.origin, botLeftRay.direction * teleportDistance, Color.green);
    }

    private void OnCollisionEnter2D(Collision2D collision)
    {
        if (collision.gameObject.CompareTag("Projectiles"))
        {
            if (collision.gameObject.name == "Arrow(Clone)")
            {
                Arrow arrow = collision.gameObject.GetComponent<Arrow>();
                Vector2 knockbackDir = rb.position - (Vector2)collision.gameObject.transform.position;
                knockbackDir.Normalize();
                TakeHit(knockbackDir * arrow.getKnockback(), arrow.getDamage());
                Destroy(collision.gameObject);
            }
        }
    }
}
