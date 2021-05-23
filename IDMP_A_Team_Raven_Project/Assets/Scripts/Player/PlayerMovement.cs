using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class PlayerMovement : MonoBehaviour
{
    // player FSM
    public enum State
    {
        Idle,
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
    public GameObject PauseCanvas;
    private PauseManager pauseScript;

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
    public float attack1AnimEnd;
    public float attack1ComboBefore;
    public float attack2ComboAfter;
    public float attack2AnimEnd;
    public float attack2ComboBefore;
    public float attack3ComboAfter;
    public float attack3AnimEnd;
    public float attack3ComboBefore;

    // movement-related vars
    public Vector2 movementDir;
    private Vector2 atkMoveDir;
    private float moveMagnitude;
    public float moveSpeed;
    public float velocityLerp;
    public float dashSpeed;
    public float dashCooldown;
    private bool ableToDash;
    public float attackMoveSpeed;
    public float healMoveSpeedFactor;

    public float dashTime;
    public float healTime;

    private float stairsVelOffset = 0f;
    private bool vulnerable;

    // control scheme using new input system
    private PlayerControls playerControls;

    public float percentageOfAFullBolt;

    private void Awake()
    {
        playerControls = new PlayerControls();
        shootScript.setControls(playerControls);
        pauseScript = PauseCanvas.GetComponent<PauseManager>();
        pauseScript.setControls(playerControls);
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

    [Header("Health Variables")]
    [SerializeField] private PlayerInventory playerInventory;
    [SerializeField] private SignalSender addPlayerHealthSignal;
    [SerializeField] private SignalSender addBoltFromInv;
    [SerializeField] private InventoryItem healthpotion;
    [SerializeField] private InventoryItem bolt;


    // Start is called before the first frame update
    void Start()
    {
        attackState = AttackState.Idle;
        rb2d = GetComponent<Rigidbody2D>();
        playerAnimator = GetComponent<Animator>();
        playerAnimator.SetFloat("MoveX", 0);
        playerAnimator.SetFloat("MoveY", -1);
        sr = GetComponent<SpriteRenderer>();
        state = State.Idle;
        attackTimeElapsed = 0f;
        vulnerable = true;
        ableToDash = true;
    }

    // Update is called once per frame
    void Update()
    {
        ProcessInputs();
    }

    private void FixedUpdate()
    {
        UpdateAnimation();

        // check for whether player is on horizontal stairs, adjust velocity offset to compensate
        RaycastHit2D hit = Physics2D.Raycast(rayOrigin.transform.position, Vector2.zero);
        if (hit && hit.collider.tag == "StairsDownRight")
        {
            stairsVelOffset = -movementDir.x;
        }
        else if (hit && hit.collider.tag == "StairsDownLeft")
        {
            stairsVelOffset = movementDir.x;
        }
        else
        {
            stairsVelOffset = 0f;
        }

        // if user input a dash since last FixedUpdate, dash then reset input bool, else just walk
        switch (state)
        {
            case State.Moving:
                if (inputDash && ableToDash)
                {
                    Dash();
                    state = State.Dashing;
                    inputDash = false;
                }
                else if (inputAttack && !shootScript.IsAiming())
                {
                    state = State.Attacking;
                    atkMoveDir = movementDir;
                    Attack();
                    inputAttack = false;
                }
                else if (inputHeal)
                {
                    if (playerInventory.playerInventory.Contains(healthpotion))
                    {
                        StartCoroutine(Heal());
                        state = State.Healing;
                        inputHeal = false;
                    }
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
                AttackMovement();
                break;
            case State.Healing:
                HealingMovement();
                // exits state upon completion of Heal() coroutine
                break;
            case State.Dashing:
                // exits state upon completion of DashCoroutine()
                break;
            case State.Aiming:
                break;
        }

        //reset input bools
        inputHeal = false;
        inputAttack = false;
        inputDash = false;
    }

    private void LateUpdate()
    {

    }

    private void ProcessInputs()
    {
        // get movement inputs
        movementDir = playerControls.Player.Move.ReadValue<Vector2>();
        // clamp magnitude for analog directional inputs (i.e. stick) and normalize diagonal inputs
        moveMagnitude = Mathf.Clamp(movementDir.magnitude, 0.0f, 1.0f);
        movementDir.Normalize();

        // check for dash input
        if (ableToDash && state == State.Moving)
        {
            playerControls.Player.Dash.started += _ => inputDash = true;
        }

        // check for attack input
        if (!shootScript.IsAiming() && state == State.Moving)
        {
            playerControls.Player.Attack.started += _ => inputAttack = true;
        }

        // check for heal input
        if (state == State.Moving)
        {
            playerControls.Player.Heal.started += _ => inputHeal = true;
        }
    }

    private void Attack()
    {
        switch (attackState)
        {
            case AttackState.Idle:
                attackCoroutine = StartCoroutine(AttackCoroutine(attack1ComboBefore, attack1AnimEnd, "Attacking"));
                attackState = AttackState.One;
                break;
            case AttackState.One:
                if (attackTimeElapsed >= attack1ComboAfter && attackTimeElapsed < attack1ComboBefore)
                {
                    StopCoroutine(attackCoroutine);
                    playerAnimator.SetBool(attack1name, false);
                    attackCoroutine = StartCoroutine(AttackCoroutine(attack2ComboBefore, attack2AnimEnd, "Attacking2"));
                    attackState = AttackState.Two;
                }
                break;
            case AttackState.Two:
                if (attackTimeElapsed >= attack2ComboAfter && attackTimeElapsed < attack2ComboBefore)
                {
                    StopCoroutine(attackCoroutine);
                    playerAnimator.SetBool(attack2name, false);
                    attackCoroutine = StartCoroutine(AttackCoroutine(attack3ComboBefore, attack3AnimEnd, "Attacking3"));
                    attackState = AttackState.Three;
                }

                break;
        }
    }

    private void Movement()
    {
        // Lerp current velocity to new velocity
        rb2d.velocity = Vector2.Lerp(rb2d.velocity, new Vector2(movementDir.x, stairsVelOffset + movementDir.y) * moveMagnitude * moveSpeed, velocityLerp * Time.deltaTime);
    }

    private void HealingMovement()
    {
        // decay current velocity to 0 (no moving when healing)
        rb2d.velocity = Vector2.Lerp(rb2d.velocity, new Vector2(movementDir.x, stairsVelOffset + movementDir.y) * moveMagnitude * moveSpeed * healMoveSpeedFactor, velocityLerp * Time.deltaTime);
    }

    private void AttackMovement()
    {
        rb2d.velocity = Vector2.Lerp(rb2d.velocity, new Vector2(atkMoveDir.x, stairsVelOffset + atkMoveDir.y) * moveMagnitude * moveSpeed * 0.3f, velocityLerp * Time.deltaTime);
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
            if (state == State.Attacking)
            {
                Debug.Log("changing attack dir");
                if (Mathf.Abs(atkMoveDir.x) > Mathf.Abs(atkMoveDir.y))
                {
                    playerAnimator.SetFloat("MoveX", atkMoveDir.x * (1 / Mathf.Abs(atkMoveDir.x)));
                    playerAnimator.SetFloat("MoveY", 0);
                }
                else
                {
                    playerAnimator.SetFloat("MoveX", 0);
                    playerAnimator.SetFloat("MoveY", atkMoveDir.y * (1 / Mathf.Abs(atkMoveDir.y)));
                }
                playerAnimator.SetBool("Moving", false);
            }
            else
            {
                if (shootScript.IsAiming())
                {
                    playerAnimator.SetFloat("MoveX", movementDir.x);
                    playerAnimator.SetFloat("MoveY", movementDir.y);
                    playerAnimator.SetBool("Moving", true);
                    playerAnimator.SetBool("Aiming", true);
                }
                else
                {
                    playerAnimator.SetFloat("MoveX", movementDir.x);
                    playerAnimator.SetFloat("MoveY", movementDir.y);
                    playerAnimator.SetBool("Moving", true);
                    playerAnimator.SetBool("Aiming", false);
                }
            }
        }
        else
        {
            playerAnimator.SetBool("Moving", false);
        }
    }

    IEnumerator DashCoroutine()
    {
        vulnerable = false;
        ableToDash = false;
        float elapsed = 0f;
        while (elapsed < dashTime)
        {
            rb2d.velocity = new Vector2(movementDir.x, stairsVelOffset + movementDir.y) * dashSpeed;
            elapsed += Time.deltaTime;
            yield return null;
        }
        vulnerable = true;
        state = State.Moving;
        StartCoroutine(DashCooldown());
    }

    IEnumerator DashCooldown()
    {
        ableToDash = false;
        float elapsed = 0f;
        while (elapsed < dashCooldown)
        {
            elapsed += Time.deltaTime;
            yield return null;
        }
        ableToDash = true;
    }

    IEnumerator AttackCoroutine(float attackComboBefore, float attackAnimEnd, string attackName)
    {
        attackTimeElapsed = 0f;
        playerAnimator.SetBool(attackName, true);
        rb2d.velocity = Vector2.zero;
        while (attackTimeElapsed < attackAnimEnd)
        {
            attackTimeElapsed += Time.deltaTime;
            yield return null;
        }
        playerAnimator.SetBool(attackName, false);

        while (attackTimeElapsed < attackComboBefore)
        {
            attackTimeElapsed += Time.deltaTime;
            yield return null;
        }

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
            yield return null;
        }
        sr.color = new Color(1f, 1f, 1f, 1f);
        state = State.Moving;
        healthpotion.Use();
        addPlayerHealthSignal.Raise();
    }

    public PlayerControls getControls()
    {
        return playerControls;
    }

    public bool isVulnerable()
    {
        return this.vulnerable;
    }

    public void addBoltToInv()
    {
        percentageOfAFullBolt += 0.2f;
        if (percentageOfAFullBolt == 1.0f)
        {
            playerInventory.AddItem(bolt);
            percentageOfAFullBolt = 0.0f;
        }
        addBoltFromInv.Raise();
    }
}
