using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecromancerScript : RangedEnemy
{
    GridAreaScript gridScript;

    public float castingDelay;              //The delay between casting the attack and strating it
    public float attackDelay;               //The delay between casting attacks

    private float castingTimer = 0f; 
    private float attackTimer = 0f;
    private GameObject playerObject;

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


        //Play a random pattern
        if (attackTimer < attackDelay)
            attackTimer += Time.deltaTime;
        else {
            if (!gridScript.isCasting) {
                attackTimer = 0;
                animator.SetBool("isAttacking", true);
                gridScript.playRandomPattern();
            }
        }

        //Teleport when the player is too close
        if(Vector2.Distance(playerObject.transform.position, transform.position) < 2f) {
            coroutine = StartCoroutine(startTeleport());
        }
    }

    void stopCasting() {
        animator.SetBool("isAttacking", false);
    }


}
