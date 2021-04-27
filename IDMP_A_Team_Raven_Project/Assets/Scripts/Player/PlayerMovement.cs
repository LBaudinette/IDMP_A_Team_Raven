using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{

    public enum State
    {
        Moving,
        Aiming,
        Attacking,
        Healing,
        Dashing
    }

    public State state;

    private Rigidbody2D rb2d;
    private Animator playerAnimator;
    private SpriteRenderer sr;

    public bool inputHeal;
    private bool inputDash;
    private bool inputAttack;
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
    public float healTime;
    public float healMoveSpeedFactor;

    private PlayerControls playerControls;

    private void Awake()
    {
        playerControls = new PlayerControls();
        shootScript.setControls(playerControls);
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
        sr = GetComponent<SpriteRenderer>();
        state = State.Moving;
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
        switch (state)
        {
            case State.Moving:
                if (inputDash)
                {
                    Dash();
                    inputDash = false;
                }
                else if (inputAttack)
                {
                    Attack();
                    inputAttack = false;
                } else if (inputHeal)
                {
                    StartCoroutine(Heal());
                    state = State.Healing;
                }
                else
                {
                    Movement();
                }
                break;
            case State.Attacking:
                break;
            case State.Aiming:
                break;
            case State.Healing:
                HealingMovement();
                break;
            case State.Dashing:
                break;
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
            playerControls.Player.Dash.started += _ => inputDash = true;
        }

        // check for attack input
        playerControls.Player.Attack.started += _ => inputAttack = true;

        playerControls.Player.Heal.started += _ => inputHeal = true;

        
    }

    private void Attack()
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

    private void Movement()
    {
        // Lerp current velocity to new velocity
        rb2d.velocity = Vector2.Lerp(rb2d.velocity, new Vector2(movementDir.x, movementDir.y) * moveMagnitude * moveSpeed, velocityLerp * Time.deltaTime);
    }

    private void HealingMovement()
    {
        // decay current velocity to 0 (no moving when healing)
        rb2d.velocity = Vector2.Lerp(rb2d.velocity, new Vector2(movementDir.x, movementDir.y) * moveMagnitude * moveSpeed * healMoveSpeedFactor, velocityLerp * Time.deltaTime);
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
        } else if (attackNum == 2)
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
        }

        isAttacking = false;
        isAttacking1 = false;
        isAttacking2 = false;

    }

    IEnumerator Heal()
    {
        state = State.Healing;
        float elapsed = 0f;
        while (elapsed < healTime)
        {
            sr.color = new Color(1f - (elapsed / healTime), 1f, 1f - (elapsed / healTime), 1f);
            elapsed += Time.deltaTime;
            Debug.Log("elapsed = " + elapsed);
            yield return null;
        }
        Debug.Log("left heal loop");
        sr.color = new Color(1f, 1f, 1f, 1f);
        inputHeal = false;
        state = State.Moving;
        //playerHealth.AddHealth();
    }
    
}
