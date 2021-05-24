using UnityEngine;
using System.Linq;

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
            enemies.Concat(rangedEnemies).Concat(bosses)
                .ToList().ForEach(e => e.SetActive(true));
            virtualCamera.SetActive(true);
        }
        
    }

    //Disable relevant enemies and pathfinding grid
    public virtual void OnTriggerExit2D(Collider2D other)
    {
        //pathfindingGrid.SetActive(false);

        if (other.CompareTag("Player") && !other.isTrigger)
        {
            enemies.Concat(rangedEnemies).Concat(bosses)
                .ToList().ForEach(e => e.SetActive(false));
            virtualCamera.SetActive(false);
        }
    }

    //public void ChangeActivation(Component component, bool activation)
    //{
    //    component.gameObject.SetActive(activation);
    //}
}
