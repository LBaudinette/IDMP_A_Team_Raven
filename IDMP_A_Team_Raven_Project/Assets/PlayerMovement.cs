using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private Animator playerAnimator;

    private bool inputDash;

    public Vector2 movementDir;
    public float moveMagnitude;
    public float moveSpeed;
    public float velocityLerp;
    public float dashSpeed;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        playerAnimator.SetFloat("MoveX", 0);
        playerAnimator.SetFloat("MoveY", -1);
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInputs();
    }

    private void FixedUpdate()
    {
        UpdateAnimation();
        // if user input a dash since last FixedUpdate, dash then reset input bool, else just walk
        if (inputDash)
        {
            Dash();
            inputDash = false;
        } else
        {
            Walk();
        }
    }

    private void ProcessInputs()
    {
        // get movement inputs
        movementDir.x = Input.GetAxisRaw("Horizontal");
        movementDir.y = Input.GetAxisRaw("Vertical");

        // clamp magnitude for analog directional inputs (i.e. stick) and normalize diagonal inputs
        moveMagnitude = Mathf.Clamp(movementDir.magnitude, 0.0f, 1.0f);
        movementDir.Normalize();

        // if dash input hasn't been read since last FixedUpdate, check for dash input
        if (!inputDash)
        {
            inputDash = Input.GetButtonDown("Dash");
        }
    }

    private void Walk()
    {
        // Lerp current velocity to new velocity
        rb2d.velocity = Vector2.Lerp(rb2d.velocity, new Vector2(movementDir.x, movementDir.y) * moveMagnitude * moveSpeed, velocityLerp * Time.deltaTime);
    }

    private void Dash()
    {
        // set current velocity to zero, then dash in movement direction
        rb2d.velocity = Vector2.zero;
        rb2d.velocity += movementDir * dashSpeed;
    }

    private void UpdateAnimation()
    {
        if (movementDir != Vector2.zero)
        {
            playerAnimator.SetFloat("MoveX", movementDir.x);
            playerAnimator.SetFloat("MoveY", movementDir.y);
            playerAnimator.SetBool("Moving", true);
        }
        else
        {
            playerAnimator.SetBool("Moving", false);
        }
    }
}
