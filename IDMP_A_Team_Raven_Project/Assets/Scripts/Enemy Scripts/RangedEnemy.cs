using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class RangedEnemy : MonoBehaviour
{

    private Vector2 playerPos;
    private float firingDelay = 2f;          //The delay between firing projectiles
    private float timer = 0f;                //Keeps track of timer between shots
    public GameObject fireball;         //Prefab of projectile
    public float projForce;
    public Transform firePoint;
    public Transform rotationPoint;
    public float teleTriggerDistance = 1;          //How close the player must be to start teleporting

    private float teleportTimer = 0;     
    public float teleportDelay = 3;     //How long it takes to teleport
    public float teleportDistance = 3f;
    private bool isTeleporting = false;

    public Transform leftRaycastPoint, topRaycastPoint, 
        rightRaycastPoint, botRaycastPoint;
    public LayerMask layerDetection;

    private Coroutine coroutine;

    // Start is called before the first frame update
    void Start()
    {
        playerPos = GameObject.FindWithTag("Player").transform.position;
    }

    // Update is called once per frame
    void Update()
    {
        //Update the player position every frame
        playerPos = GameObject.FindWithTag("Player").transform.position;

        //Check to see if the Enemy can fire a projectile
        if (timer <= 0) {
            fireProjectile();
            timer = firingDelay;
        }
        else
            timer -= Time.deltaTime;

        //Check if the player is within distance to teleport
        if (Vector2.Distance(transform.position, playerPos) < teleTriggerDistance && !isTeleporting)
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


    private IEnumerator startTeleport() {

        isTeleporting = true;
        Debug.Log("Starting Teleport");

        //Start the teleport timer
        while(teleportTimer < teleportDelay) {
            Debug.Log("Timer: " + teleportTimer);
            teleportTimer += Time.deltaTime;

            yield return null;
        }

        

        teleport();
    }

    void teleport() {
        //Teleport
        //Fire 4 raycasts to check for available teleports
        RaycastHit2D leftHit;
        RaycastHit2D topHit;
        RaycastHit2D rightHit;
        RaycastHit2D botHit;

        leftHit = Physics2D.Raycast(leftRaycastPoint.position, Vector2.left, teleTriggerDistance, layerDetection);
        topHit = Physics2D.Raycast(topRaycastPoint.position, Vector2.up, teleTriggerDistance, layerDetection);
        rightHit = Physics2D.Raycast(rightRaycastPoint.position, Vector2.right, teleTriggerDistance, layerDetection);
        botHit = Physics2D.Raycast(botRaycastPoint.position, Vector2.down, teleTriggerDistance, layerDetection);

        //Teleport the furthest distance

        transform.Translate(Vector2.left * teleTriggerDistance);

        isTeleporting = false;
        //Reset teleport timer
        teleportTimer = 0;
    }
}
