using UnityEngine;

public class Room : MonoBehaviour
{
    public GameObject virtualCamera;
    //public GameObject pathfindingGrid;
    public GameObject[] enemies;

    //Enable relevant enemies and pathfinding grid
    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        //pathfindingGrid.SetActive(true);
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            //Activate all enemies
            for (int i = 0; i < enemies.Length; i++)
            {
                //ChangeActivation(enemies[i], true);
                //Instantiate(enemies[i], transform);
                if(!enemies[i].GetComponent<Enemy>().isDead)
                    enemies[i].SetActive(true);


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
            //Deactivate all enemires
            for (int i = 0; i < enemies.Length; i++)
            {
                //ChangeActivation(enemies[i], false);
                enemies[i].SetActive(false);
            }
            virtualCamera.SetActive(false);
        }
    }

    //public void ChangeActivation(Component component, bool activation)
    //{
    //    component.gameObject.SetActive(activation);
    //}
}
