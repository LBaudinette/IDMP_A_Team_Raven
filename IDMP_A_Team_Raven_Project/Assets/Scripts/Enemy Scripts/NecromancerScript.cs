using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecromancerScript : RangedEnemy
{
    GridAreaScript gridScript;
    private GameObject playerObject;
    private bool canAttack = true;


    //private Animator animator;
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
        if (health <= 0)
            animator.SetBool("isDead", true);
        health -= damage;
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
