using UnityEngine;

public class Room : MonoBehaviour
{
    public GameObject virtualCamera;
    //public GameObject pathfindingGrid;
    public GameObject[] enemies;
    public GameObject[] rangedEnemies;
    public GameObject[] bosses;

    //Enable relevant enemies and pathfinding grid
    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            //Set all melee enemies active
            for (int i = 0; i < enemies.Length; i++)
            {
                //ChangeActivation(enemies[i], true);
                if (!enemies[i].GetComponent<Enemy>().isDead)
                    enemies[i].SetActive(true);


            }
            //Set all ranged enemies active
            for (int i = 0; i < rangedEnemies.Length; i++) {

                if (!rangedEnemies[i].GetComponent<RangedEnemy>().isDead)
                    rangedEnemies[i].SetActive(true);
                

            }
            //Set all boss enemies active
            for (int i = 0; i < bosses.Length; i++) {
                if (!bosses[i].GetComponent<NecromancerScript>().isDead)
                    bosses[i].SetActive(true);
            }


            virtualCamera.SetActive(true);
        }
        
    }

    //Disable relevant enemies and pathfinding grid
    public virtual void OnTriggerExit2D(Collider2D other)
    {
        //pathfindingGrid.SetActive(false);

        if (other.CompareTag("Player") && !other.isTrigger)
        {

            //Deactivate all melee enemies active
            for (int i = 0; i < enemies.Length; i++) {
                //ChangeActivation(enemies[i], true);
                if (!enemies[i].GetComponent<Enemy>().isDead)
                    enemies[i].SetActive(false);


            }
            //Deactivate all ranged enemies active
            for (int i = 0; i < rangedEnemies.Length; i++) {

                if (!rangedEnemies[i].GetComponent<RangedEnemy>().isDead)
                    rangedEnemies[i].SetActive(false);


            }
            //Deactivate all boss enemies active
            for (int i = 0; i < bosses.Length; i++) {
                if (!bosses[i].GetComponent<NecromancerScript>().isDead)
                    bosses[i].SetActive(false);
            }
            virtualCamera.SetActive(false);
        }
    }

    //public void ChangeActivation(Component component, bool activation)
    //{
    //    component.gameObject.SetActive(activation);
    //}
}
