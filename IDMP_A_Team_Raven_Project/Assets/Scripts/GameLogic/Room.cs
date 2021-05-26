using UnityEngine;
using System.Collections.Generic;
using System.Linq;

public class Room : MonoBehaviour
{
    public GameObject virtualCamera;
    //public GameObject pathfindingGrid;
    public List<GameObject> enemies;
    public List<GameObject> rangedEnemies;
    public List<GameObject> bosses;
    public MusicManager musicManager;

    private int enemyCount;

    private void Start()
    {
        enemyCount = enemies.Count + rangedEnemies.Count + bosses.Count;
    }

    public void enemyDied()
    {
        enemyCount--;
        if (enemyCount <= 0)
        {
            onBattleEnd();
        }
    }

    public void enemyRevived()
    {
        enemyCount++;
    }

    private void onBattleEnd()
    {
        musicManager.fadeToAmbient();
    }

    private void onBattleStart()
    {
        musicManager.fadeToBattle();
    }

    //Enable relevant enemies and pathfinding grid
    public virtual void OnTriggerEnter2D(Collider2D other)
    {
        if (other.CompareTag("Player") && !other.isTrigger)
        {
            enemies.Concat(rangedEnemies).Concat(bosses)
                .ToList().ForEach(e => e.SetActive(true));
            virtualCamera.SetActive(true);
            if (enemyCount > 0)
            {
                if (!musicManager.isInBattle())
                {
                    onBattleStart();
                }
            } else
            {
                if (musicManager.isInBattle())
                {
                    onBattleEnd();
                }
            }
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
