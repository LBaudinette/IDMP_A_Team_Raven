using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // player FSM
    public enum State
    {
        Moving,
        Aiming,
        Attacking,
        Healing,
        Dashing
    }
    public State state;

    // attached components and scripts
    private Rigidbody2D rb2d;
    private Animator playerAnimator;
    private SpriteRenderer sr;
    public PlayerShoot shootScript;

    // vars for tracking player inputs
    public bool inputHeal;
    private bool inputDash;
    private bool inputAttack;

    // attack states
    private enum AttackState
    {
        Idle,
        One,
        Two,
        Three
    }
    private AttackState attackState;

    // vars for tracking player attack states
    private Coroutine attackCoroutine;
    public string attack1name;
    public string attack2name;
    public string attack3name;

    // attack timing
    private float attackTimeElapsed;
    public float attack1ComboAfter;
    public float attack1Max;
    public float attack2ComboAfter;
    public float attack2Max;
    public float attack3ComboAfter;
    public float attack3Max;

    // movement-related vars
    private Vector2 movementDir;
    private float moveMagnitude;
    public float moveSpeed;
    public float velocityLerp;
    public float dashSpeed;
    public float attackMoveSpeed;
    public float healMoveSpeedFactor;

    public float dashTime;
    public float healTime;

    private float stairsVelOffset = 0f;

    // control scheme using new input system
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

    public GameObject rayOrigin;
    private string directionAmender = "normal";

    [Header("Health Variables")]
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private SignalSender addPlayerHealthSignal;
    [SerializeField] private InventoryItem healthpotion;


    // Start is called before the first frame update
    void Start()
    {
        attackState = AttackState.Idle;
        rb2d = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        playerAnimator.SetFloat("MoveX", 0);
        playerAnimator.SetFloat("MoveY", -1);
        sr = GetComponent<SpriteRenderer>();
        state = State.Moving;

        attackTimeElapsed = 0f;
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInputs();
    }

    private void FixedUpdate()
    {
        UpdateAnimation();

        RaycastHit2D hit = Physics2D.Raycast(rayOrigin.transform.position, Vector2.zero);

        if (hit && hit.collider.tag == "StairsDownRight")
        {
            directionAmender = "stairsDownRight";
            stairsVelOffset = -movementDir.x;
        } else if (hit && hit.collider.tag == "StairsDownLeft")
        {
            directionAmender = "stairsDownLeft";
            stairsVelOffset = movementDir.x;
        }
        else 
        {
            directionAmender = "normal";
            stairsVelOffset = 0f;
        }

        // if user input a dash since last FixedUpdate, dash then reset input bool, else just walk
        switch (state)
        {
            case State.Moving:
                if (inputDash)
                {
                    Dash();
                    state = State.Dashing;
                    inputDash = false;
                }
                else if (inputAttack && !shootScript.IsAiming())
                {
                    state = State.Attacking;
                    Attack();
                    inputAttack = false;
                } else if (inputHeal)
                {
                    StartCoroutine(Heal());
                    state = State.Healing;
                    inputHeal = false;
                }
                else
                {
                    Movement();
                }
                break;
            case State.Attacking:
                if (inputAttack)
                {
                    Attack();
                    inputAttack = false;
                }
                break;
            case State.Healing:
                HealingMovement();
                // exits state upon completion of Heal() coroutine
                break;
            case State.Dashing:
                // exits state upon completion of DashCoroutine()
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

        // check for dash input
        playerControls.Player.Dash.started += _ => inputDash = true;

        // check for attack input
        playerControls.Player.Attack.started += _ => inputAttack = true;

        // check for heal input
        playerControls.Player.Heal.started += _ => inputHeal = true;

        if (Input.GetButtonDown("Heal"))
        {
            if (playerInventory.playerInventory.Contains(healthpotion))
            {
                healthpotion.Use();
                addPlayerHealthSignal.Raise();
            }
        }
    }

    private void Attack()
    {
        switch (attackState)
        {
            case AttackState.Idle:
                attackCoroutine = StartCoroutine(AttackCoroutine(attack1Max, "Attacking"));
                attackState = AttackState.One;
                break;
            /*case AttackState.One:
                if (attackTimeElapsed >= attack1ComboAfter && attackTimeElapsed < attack1Max)
                {
                    StopCoroutine(attackCoroutine);
                    //attackCoroutine = StartCoroutine(AttackCoroutine(attack2Max, "Attacking2"));
                    attackState = AttackState.Two;
                }
                break;
            case AttackState.Two:
                if (attackTimeElapsed >= attack2ComboAfter && attackTimeElapsed < attack2Max)
                {
                    StopCoroutine(attackCoroutine);
                    //attackCoroutine = StartCoroutine(AttackCoroutine(attack3Max, "Attacking3"));
                    attackState = AttackState.Three;
                }
                
                break;*/
        }
    }

    private void Movement()
    {
        // Lerp current velocity to new velocity
        rb2d.velocity = Vector2.Lerp(rb2d.velocity, new Vector2(movementDir.x, movementDir.y) * moveMagnitude * moveSpeed, velocityLerp * Time.deltaTime);
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
        StartCoroutine(DashCoroutine());
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

    IEnumerator DashCoroutine()
    {
        float elapsed = 0f;
        while (elapsed < dashTime)
        {
            rb2d.velocity = movementDir * dashSpeed;
            elapsed += Time.deltaTime;
            yield return null;
        }
        state = State.Moving;
    }

    IEnumerator AttackCoroutine(float attackMaxTime, string attackName)
    {
        attackTimeElapsed = 0f;
        playerAnimator.SetBool(attackName, true);

        while (attackTimeElapsed < attackMaxTime)
        {
            attackTimeElapsed += Time.deltaTime;
            yield return null;
        }
        playerAnimator.SetBool(attackName, false);
        attackState = AttackState.Idle;
        state = State.Moving;
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
        state = State.Moving;
        //playerHealth.AddHealth();
    }
}
