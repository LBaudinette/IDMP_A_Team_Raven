using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecromancerScript : RangedEnemy
{
    GridAreaScript gridScript;
    private GameObject playerObject;
    private bool canAttack = true;

    [Header("Boss Health Variables")]
    public SignalSender bossHealthSignal;
    public GameObject bossHealthBar;

    //private Animator animator;

    private void OnEnable()
    {
        bossHealthBar.SetActive(true);
    }

    // Start is called before the first frame update
    void Start()
    {
        gridScript = GameObject.FindWithTag("GridArea").GetComponent<GridAreaScript>();
        animator = GetComponent<Animator>();
        playerObject = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        Ray2D leftRay = new Ray2D(leftRaycastPoint.position, Vector2.left);
        Ray2D topLeftRay = new Ray2D(topLeftRaycastPoint.position, new Vector2(-1, 1));
        Ray2D topRay = new Ray2D(topRaycastPoint.position, Vector2.up);
        Ray2D topRightRay = new Ray2D(topRightRaycastPoint.position, new Vector2(1, 1));
        Ray2D rightRay = new Ray2D(rightRaycastPoint.position, Vector2.right);
        Ray2D botRightRay = new Ray2D(bottomRightRaycastPoint.position, new Vector2(1, -1));
        Ray2D botRay = new Ray2D(botRaycastPoint.position, Vector2.down);
        Ray2D botLeftRay = new Ray2D(bottomLeftRaycastPoint.position, new Vector2(-1, -1));

        Debug.DrawRay(leftRay.origin, leftRay.direction * teleportDistance, Color.green);
        Debug.DrawRay(topLeftRay.origin, topLeftRay.direction * teleportDistance, Color.green);
        Debug.DrawRay(topRay.origin, topRay.direction * teleportDistance, Color.green);
        Debug.DrawRay(topRightRay.origin, topRightRay.direction * teleportDistance, Color.green);
        Debug.DrawRay(rightRay.origin, rightRay.direction * teleportDistance, Color.green);
        Debug.DrawRay(botRightRay.origin, botRightRay.direction * teleportDistance, Color.green);
        Debug.DrawRay(botRay.origin, botRay.direction * teleportDistance, Color.green);
        Debug.DrawRay(botLeftRay.origin, botLeftRay.direction * teleportDistance, Color.green);



        //Update Direction
        float direction = playerObject.transform.position.x - transform.position.x;
        if (direction < 0)
            animator.SetFloat("playerDirection", -1);
        else if (direction > 0)
            animator.SetFloat("playerDirection", 1);

        if (teleportCDTimer < teleportCooldown)
            teleportCDTimer += Time.deltaTime;
        else {
            canTeleport = true;
            teleportCDTimer = 0f;
        }
            

        if (attackTimer < attackDelay)
            attackTimer += Time.deltaTime;
        else if(attackTimer >= attackDelay && !gridScript.isCasting && !isTeleporting)
            canAttack = true;

        if (canAttack) {
            attackTimer = 0f;
            animator.SetBool("isAttacking", true);
            canAttack = false;
        }


        //Teleport when the player is too close
        if (Vector2.Distance(playerObject.transform.position, transform.position) < teleTriggerDistance && 
            canTeleport) {
            coroutine = StartCoroutine(startTeleport());
        }
    }


    public void TakeHit(float damage) {
        health -= damage;
        bossHealthSignal.Raise();
        if (health <= 0)
        {
            animator.SetBool("isDead", true);
            bossHealthBar.SetActive(false);
        }
    }

    //protected override void onDeath() {
    //    //rb.bodyType = RigidbodyType2D.Static;
    //    Destroy(gameObject);
    //}

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Hitbox") {
            DealHitMelee hitbox = collision.GetComponent<DealHitMelee>();
            TakeHit(hitbox.getDamage());
        }
    }

    void playAttack() {
        gridScript.playRandomPattern();
        animator.SetBool("isAttacking", false);

    }
}
