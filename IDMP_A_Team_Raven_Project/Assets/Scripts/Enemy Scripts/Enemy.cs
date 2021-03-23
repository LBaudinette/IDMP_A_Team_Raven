using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemy : MonoBehaviour
{
    protected Animator animator;
    protected bool canAttack;

    // Start is called before the first frame update
    void Start()
    {
        animator = GetComponent<Animator>();
    }

    // Update is called once per frame
    void Update()
    {
        //
    }

    protected void startMoving(Transform target) {

    }

    protected virtual void startAttack() {

    }
}
