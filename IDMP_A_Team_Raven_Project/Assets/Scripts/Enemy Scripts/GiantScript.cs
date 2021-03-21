using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class GiantScript : MonoBehaviour
{
    public GameObject areaAttack;               //The area prefab that is used for the attack
    public float attackDistance;                /*The distance from the player that 
                                                the enemy has to be to make an attack*/
    private GameObject player;
    private float attackDelay = 1f;
    private float attackTimer = 0;
    private bool canAttack = true;

    // Start is called before the first frame update
    void Start()
    {
        player = GameObject.FindWithTag("Player");
    }

    // Update is called once per frame
    void Update()
    {
        float distance = Vector2.Distance(transform.position, player.transform.position);
        if (distance <= attackDistance)
            startAttack();
    }

    private void startAttack() {
        if()
        Instantiate(areaAttack, transform.position, transform.rotation);
    }

    private
}
