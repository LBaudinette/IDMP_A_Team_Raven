using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb2d;

    public Vector2 movementDir;
    public float moveMagnitude;
    public float moveSpeed;
    public float velLerp;

    // Start is called before the first frame update
    void Start()
    {
        rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInputs();
    }

    private void FixedUpdate()
    {
        Move();
    }

    private void ProcessInputs()
    {
        // get movement inputs
        movementDir.x = Input.GetAxisRaw("Horizontal");
        movementDir.y = Input.GetAxisRaw("Vertical");

        // clamp magnitude for analog directional inputs (i.e. stick) and normalize diagonal inputs
        moveMagnitude = Mathf.Clamp(movementDir.magnitude, 0.0f, 1.0f);
        movementDir.Normalize();
    }

    private void Move()
    {
        // Lerp current velocity to new velocity
        rb2d.velocity = Vector2.Lerp(rb2d.velocity, new Vector2(movementDir.x, movementDir.y) * moveMagnitude * moveSpeed, velLerp * Time.deltaTime);
    }
}
