using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantScript : MonoBehaviour
{
    public GameObject areaAttack;               //The area prefab that is used for the attack
    public float attackDistance;                /*The distance from the player that 
                                                the enemy has to be to make an attack*/
    private GameObject player;
    public float attackDelay = 1f;
    private float attackTimer = 0;
    private bool canAttack = true;
    private Coroutine coroutine;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        //check for the distance between the enemy and the player
        float distance = Vector2.Distance(transform.position, 
            player.transform.position);
        if (distance <= attackDistance)
            //if the enemy is close enough to the player, attack
            startAttack();
    }

    private void startAttack() {
        //create a special area if enough time has passed between attacks
        if (canAttack) {
            Debug.Log("Attack");
            Instantiate(areaAttack, transform.position, transform.rotation);
            canAttack = false;
            coroutine = StartCoroutine(startAttackCooldown());
        }
    }
    
    private IEnumerator startAttackCooldown() {
        while(attackTimer < attackDelay) {
            attackTimer += Time.deltaTime;

            yield return null;
        }

        canAttack = true;
        attackTimer = 0f;
    }

    public void takeDamage(float damage, float force, Vector2 angle) {

    }

}
