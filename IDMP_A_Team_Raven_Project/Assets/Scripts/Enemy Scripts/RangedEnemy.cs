using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : MonoBehaviour {

    private Vector2 playerPos;
    protected float attackDelay = 2f;          //The delay between firing projectiles
    protected float attackTimer = 0f;                //Keeps track of timer between shots
    public GameObject fireball;         //Prefab of projectile
    public float projForce;
    public Transform firePoint;
    public Transform rotationPoint;
    public float teleTriggerDistance = 1;          //How close the player must be to start teleporting

    public float health = 100f;

    protected float teleportTimer = 0;
    protected float teleportCDTimer = 0;
    public float teleportDelay = 3;     //How long it takes to teleport
    public float teleportDistance = 3f;
    public float teleportCooldown = 4f;
    protected bool isTeleporting = false;
    protected bool canTeleport = true;

    public Transform leftRaycastPoint, topLeftRaycastPoint, topRaycastPoint,
        topRightRaycastPoint, rightRaycastPoint, bottomRightRaycastPoint,botRaycastPoint,
        bottomLeftRaycastPoint;
    public LayerMask layerDetection;


    protected Animator animator;
    protected Rigidbody2D rb;
    protected Coroutine coroutine;

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
        playerPos = GameObject.FindWithTag("Player").transform.position;
        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update() {
        //Raycast debugging
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

        //Update the player position every frame
        playerPos = GameObject.FindWithTag("Player").transform.position;

        //Check to see if the Enemy can fire a projectile
        if (attackTimer < attackDelay)
            attackTimer += Time.deltaTime;
        else {
            fireProjectile();
            attackTimer = 0f;
        }

        if (teleportCDTimer < teleportCooldown)
            teleportCDTimer += Time.deltaTime;
        else {
            canTeleport = true;
            teleportCDTimer = 0f;
        }

        //Check if the player is within distance to teleport
        if (Vector2.Distance(transform.position, playerPos) < teleTriggerDistance
            && canTeleport)
            coroutine = StartCoroutine(startTeleport());
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


    protected IEnumerator startTeleport() {

        isTeleporting = true;
        canTeleport = false;
        animator.SetBool("isTeleporting", true);
        //Debug.Log("Starting Teleport");

        //Start the teleport timer
        while (teleportTimer < teleportDelay) {
            //Debug.Log("TIMER: " + teleportTimer);
            teleportTimer += Time.deltaTime;

            yield return null;
        }


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

        Debug.Log("NUMBER OF COLLISIONS: " + hitRaycasts.Count);
        Debug.Log("NUMBER OF EMPTY SPACES: " + emptyRaycasts.Count);

        Raycast longestRaycast;

        //if no walls were hit, then teleport into an open space
        if (emptyRaycasts.Count != 0) {
            //assign the first element as the longest
            longestRaycast = emptyRaycasts[0];

            //foreach (Raycast raycast in emptyRaycasts) {
            //    //Debug.Log(raycast.ray.direction);
            //    if (raycast.raycastHit.distance > longestRaycast.raycastHit.distance)
            //        longestRaycast = raycast;
            //}

            //pick a random direction
            int randomIndex = Random.Range(0, emptyRaycasts.Count);

            //Teleport to the open space
            //transform.Translate(longestRaycast.ray.direction * teleportDistance);
            Debug.Log("TELEPORT INTO OPEN SPACE");
            transform.Translate(emptyRaycasts[randomIndex].ray.direction * teleportDistance);
        }
        //if there are no empty spaces, teleport next to a wall
        else {
            longestRaycast = hitRaycasts[0];

            foreach (Raycast raycast in hitRaycasts) {
                if (raycast.raycastHit.distance > longestRaycast.raycastHit.distance)
                    longestRaycast = raycast;
            }

            Debug.Log("TELEPORT NEAR WALL");

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
        StopCoroutine(coroutine);
        animator.speed = 0f;
        Destroy(this);
    }

    protected virtual void TakeHit(Vector2 velocity, float damage) {
        rb.AddForce(velocity * 5);
        if (health <= 0)
            animator.SetBool("isDead", true);
        health -= damage;
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Hitbox") {
            DealHitMelee hitbox = collision.GetComponent<DealHitMelee>();
            Vector2 knockbackDir = rb.position - (Vector2)hitbox.getParentPos().transform.position;
            knockbackDir.Normalize();
            TakeHit(knockbackDir * hitbox.getKnockback(), hitbox.getDamage());
        }
    }


}
