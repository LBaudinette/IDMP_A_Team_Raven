using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using Pathfinding;

public class Enemy : MonoBehaviour
{
    protected Animator animator;
    protected bool canAttack;
    protected Rigidbody2D rb;

    public Transform leftPlayer, rightPlayer;

    Path path;
    Seeker seeker;
    int currentWaypoint = 0;
    float speed = 200f;
    bool isEndOfPath = false;

    // Start is called before the first frame update
    void Start()
    {
        //TODO: assign leftPlayer and rightPlayer from the children of player object

        animator = GetComponent<Animator>();
        rb = GetComponent<Rigidbody2D>();
        seeker.StartPath(rb.position, leftPlayer.position, OnPathComplete);
    }

    // Update is called once per frame
    void Update()
    {
        if (path == null)
            return;

        if(currentWaypoint >= path.vectorPath.Count) {
            isEndOfPath = true;
            return;
        }
    }

    protected void OnPathComplete(Path p) {

        //if there is no error in the calculated path
        if (!p.error) {
            path = p;

        }
    }

    protected void startMoving(Transform target) {

    }

    protected virtual void startAttack() {

    }
}
