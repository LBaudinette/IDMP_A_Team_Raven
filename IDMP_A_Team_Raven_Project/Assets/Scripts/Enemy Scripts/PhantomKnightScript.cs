using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.PlayerLoop;

public class PhantomKnightScript : Enemy
{
    public GameObject lesserEnemy;              //Game object of the enemies that are spawned
    public float numEnemySpawned = 2;           //Number of smaller enemies to spawn


    protected override void Update() {
        if (Input.GetKeyDown("j"))
            onDeath();

        updateTimers();
    }
    //split into multiple lesser enemies

    protected override void onDeath() {
        gameObject.SetActive(false);
        for (int i = 0; i < numEnemySpawned; i++) {
            GameObject enemy =
                Instantiate(lesserEnemy, transform.position, transform.rotation);
            //Push new enemies away from where phantom knight dies
            //Vector2 pushVector = new Vector2(Random.Range(-1, 1), Random.Range(-1, 1)) * 500f;
            //enemy.GetComponent<Rigidbody2D>().AddForce(pushVector);
        }
        Destroy(gameObject);
    }


}
