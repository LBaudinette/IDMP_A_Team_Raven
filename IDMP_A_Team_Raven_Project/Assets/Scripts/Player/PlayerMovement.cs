using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    private Rigidbody2D rb2d;
    private Animator playerAnimator;

    private bool inputDash;
    private bool isAttacking;
    private bool isAttacking1;
    private bool isAttacking2;
    private Coroutine attackCoroutine;

    public Vector2 movementDir;
    public PlayerShoot shootScript;
    public float moveMagnitude;
    public float moveSpeed;
    public float velocityLerp;
    public float dashSpeed;
    public float attackMoveSpeed;

    private PlayerControls playerControls;

    private void Awake()
    {
        playerControls = new PlayerControls();
    }

    private void OnEnable()
    {
        playerControls.Enable();
    }

    private void OnDisable()
    {
        playerControls.Disable();
    }

    // Start is called before the first frame update
    void Start()
    {
        isAttacking = false;
        isAttacking2 = false;
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
            Move();
        }
    }

    private void ProcessInputs()
    {
        // get movement inputs
        movementDir = playerControls.Player.Move.ReadValue<Vector2>();

        // clamp magnitude for analog directional inputs (i.e. stick) and normalize diagonal inputs
        moveMagnitude = Mathf.Clamp(movementDir.magnitude, 0.0f, 1.0f);
        movementDir.Normalize();

        // if dash input hasn't been read since last FixedUpdate, check for dash input
        if (!inputDash)
        {
            playerControls.Player.Dash.performed += _ => inputDash = true;
        }

        // check for attack input
        playerControls.Player.Attack.performed += _ => inputAttack();

        
    }

    private void inputAttack()
    {
        if (!shootScript.isAiming())
        {
            Debug.Log("input attack");
            // attack
            isAttacking = true;
            if (!isAttacking1 && !isAttacking2)
            {
                isAttacking1 = true;
                attackCoroutine = StartCoroutine(AttackTimer(1));
            }
            else if (isAttacking1)
            {
                isAttacking1 = false;
                isAttacking2 = true;
                StopCoroutine(attackCoroutine);
                attackCoroutine = StartCoroutine(AttackTimer(2));
            }
            else if (isAttacking2)
            {
                isAttacking2 = false;
                StopCoroutine(attackCoroutine);
                attackCoroutine = StartCoroutine(AttackTimer(3));
            }

        }
    }

    private void Move()
    {
        // Lerp current velocity to new velocity
        Vector2 movement;
        if (isAttacking)
        {
            movement = Vector2.Lerp(rb2d.velocity, new Vector2(movementDir.x, movementDir.y) * moveMagnitude * attackMoveSpeed, velocityLerp * Time.deltaTime);
        } else
        {
            movement = Vector2.Lerp(rb2d.velocity, new Vector2(movementDir.x, movementDir.y) * moveMagnitude * moveSpeed, velocityLerp * Time.deltaTime);
        }
        rb2d.velocity = movement;
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

    IEnumerator AttackTimer(int attackNum)
    {
        rb2d.velocity /= 2;
        float elapsed = 0f;
        float max = 0f;

        if (attackNum == 1)
        {
            playerAnimator.SetBool("Attacking", true);
            max = 0.5f;
            while (elapsed < max)
            {
                elapsed += Time.deltaTime;
                yield return null;
            }
            playerAnimator.SetBool("Attacking", false);
        }/* else if (attackNum == 2)
        {
            playerAnimator.SetBool("Attacking", false);
            playerAnimator.SetBool("Attacking2", true);
            max = 0.6f;
            while (elapsed < max)
            {
                elapsed += Time.deltaTime;
                yield return null;
            }
            playerAnimator.SetBool("Attacking2", false);
        } else if (attackNum == 3)
        {
            playerAnimator.SetBool("Attacking2", false);
            playerAnimator.SetBool("Attacking3", true);
            max = 0.6f;
            while (elapsed < max)
            {
                elapsed += Time.deltaTime;
                yield return null;
            }
            playerAnimator.SetBool("Attacking3", false);
        }*/

        isAttacking = false;
        isAttacking1 = false;
        isAttacking2 = false;

    }
    
}
