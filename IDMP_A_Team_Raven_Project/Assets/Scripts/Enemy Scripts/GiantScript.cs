using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class GiantScript : Enemy
{
    public GameObject areaAttack;               //The area prefab that is used for the attack
    public float attackDistance = 3f;
    public Transform leftAttackPoint;
    public Transform rightAttackPoint;

    private Coroutine coroutine;



    void finishAreaAttack() {
        Transform spawnPos = directionFaced == (int)facingDirection.left ? leftAttackPoint : rightAttackPoint; 
        Instantiate(areaAttack, spawnPos.position, transform.rotation);

        isAttacking = false;
        isMoving = true;
        canMove = true;
        isCooldown = true;

        animator.SetBool("isAttacking", isAttacking);
        animator.SetBool("isMoving", true);
        updatePath();
    }



}
