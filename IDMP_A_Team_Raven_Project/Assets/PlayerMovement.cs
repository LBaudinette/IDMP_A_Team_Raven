using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private float moveHorizontal, moveVertical;
    private Rigidbody2D rb2d;

    public Vector2 movementDir;
    public float moveSpeed;
    public float MOVEMENT_BASE_SPEED = 1.0f;

    // Start is called before the first frame update
    void Start()
    {
        this.rb2d = GetComponent<Rigidbody2D>();
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInputs();
    }

    void FixedUpdate()
    {
        rb2d.AddForce(movementDir * moveSpeed * MOVEMENT_BASE_SPEED);
        
    }

    void ProcessInputs()
    {
        movementDir = new Vector2(Input.GetAxisRaw("Horizontal"), Input.GetAxisRaw("Vertical"));
        moveSpeed = Mathf.Clamp(movementDir.magnitude, 0.0f, 1.0f);
        movementDir.Normalize();
    }

}
