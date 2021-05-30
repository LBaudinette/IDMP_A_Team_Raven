using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PhantomKnightScript : Enemy
{
    public GameObject lesserEnemy;              //Game object of the enemies that are spawned
    public float numEnemySpawned = 2;           //Number of smaller enemies to spawn


    protected override void Update() {
        updateTimers();
    }

    //split into multiple lesser enemies

    private void split() {
        gameObject.SetActive(false);
        for (int i = 0; i < numEnemySpawned; i++) {                
            transform.parent.parent.GetComponent<Room>().enemies
                .Add(Instantiate(lesserEnemy, transform.position, transform.rotation, transform.parent));

        }
        //Destroy(gameObject);
        onDeath();
    }


}
