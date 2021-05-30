using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class NecromancerScript : RangedEnemy
{
    GridAreaScript gridScript;
    private GameObject playerObject;
    private bool canAttack = true;
    private bool isSecondStage = false;
    public GameObject[] bossGhouls;
    private float summonTimer = 0f;
    public float summonDelay;
    public Sprite leftDeathSprite, rightDeathSprite;

    private Coroutine timerCoroutine;

    [Header("Boss Health Variables")]
    public SignalSender bossHealthSignal;
    public GameObject bossHealthBar;

    //private Animator animator;

    private void OnEnable()
    {
        bossHealthBar.SetActive(true);
    }

    private void OnDisable()
    {
        bossHealthBar.SetActive(false);
    }

    // Start is called before the first frame update
    void Start()
    {
        maxHealth = health;
        gridScript = GameObject.FindWithTag("GridArea").GetComponent<GridAreaScript>();
        animator = GetComponent<Animator>();
        playerObject = GameObject.FindWithTag("Player");
        afterImageScript = GetComponent<AfterImageScript>();
        audio = gameObject.AddComponent<AudioSource>();
        audio.volume = 0.5f;

    }

    // Update is called once per frame
    void Update()
    {
        Ray2D leftRay = new Ray2D(leftRaycastPoint.position, Vector2.left);
        Ray2D topLeftRay = new Ray2D(topLeftRaycastPoint.position, new Vector2(-1, 1));
        Ray2D topRay = new Ray2D(topRaycastPoint.position, Vector2.up);
        Ray2D topRightRay = new Ray2D(topRightRaycastPoint.position, new Vector2(1, 1));
        Ray2D rightRay = new Ray2D(rightRaycastPoint.position, Vector2.right);
        Ray2D botRightRay = new Ray2D(bottomRightRaycastPoint.position, new Vector2(1, -1));
        Ray2D botRay = new Ray2D(botRaycastPoint.position, Vector2.down);
        Ray2D botLeftRay = new Ray2D(bottomLeftRaycastPoint.position, new Vector2(-1, -1));

        Debug.DrawRay(leftRay.origin, leftRay.direction * teleportDistance, Color.green);
        Debug.DrawRay(topLeftRay.origin, topLeftRay.direction * teleportDistance, Color.green);
        Debug.DrawRay(topRay.origin, topRay.direction * teleportDistance, Color.green);
        Debug.DrawRay(topRightRay.origin, topRightRay.direction * teleportDistance, Color.green);
        Debug.DrawRay(rightRay.origin, rightRay.direction * teleportDistance, Color.green);
        Debug.DrawRay(botRightRay.origin, botRightRay.direction * teleportDistance, Color.green);
        Debug.DrawRay(botRay.origin, botRay.direction * teleportDistance, Color.green);
        Debug.DrawRay(botLeftRay.origin, botLeftRay.direction * teleportDistance, Color.green);

        //Update Direction
        float direction = playerObject.transform.position.x - transform.position.x;
        if (direction < 0)
            animator.SetFloat("playerDirection", -1);
        else if (direction > 0)
            animator.SetFloat("playerDirection", 1);

        
            
        //Update the attack timer
        if (attackTimer < attackDelay)
            attackTimer += Time.deltaTime;
        else if(attackTimer >= attackDelay && !gridScript.isCasting && !isTeleporting)
            canAttack = true;

        //Update the summon timer
        if ((summonTimer < summonDelay) && isSecondStage)
            summonTimer += Time.deltaTime;
        else {
            summonTimer = 0;
        }

        if (canAttack) {
            attackTimer = 0f;
            animator.SetBool("isAttacking", true);
            isAttacking = true;
            canAttack = false;
        }

        if (teleportCDTimer < teleportCooldown && !isTeleporting)
            teleportCDTimer += Time.deltaTime;
        else if(!isAttacking) {
            canTeleport = true;
            teleportCDTimer = 0f;
        }

        //Teleport when the player is too close
        if (Vector2.Distance(playerObject.transform.position, transform.position) < teleTriggerDistance && 
            canTeleport && !isTeleporting && !isAttacking) {
            canTeleport = false;

            coroutine = StartCoroutine(startTeleport());
        }
    }


    public void TakeHit(float damage) {
        health -= damage;
		bossHealthSignal.Raise();
        hitPS.Play();

        //Play random hurt noise
        int randomIndex = Random.Range(0, hurtSounds.Length);
        audio.clip = hurtSounds[randomIndex];
        audio.Play();

        if (health <= maxHealth / 2 && !isSecondStage) {
            isSecondStage = true;
            Debug.Log("SECOND STAGE");
        }
        else if (health <= 0) {
            Debug.Log("DEAD");
            animator.SetBool("isDead", true);
            bossHealthBar.SetActive(false);
        }
    }

    protected override void onDeath() {
        StopAllCoroutines();
        teleportPS.Stop();
        isDead = true;
        animator.SetBool("isDead", false);
        Destroy(animator);
        if(animator.GetFloat("playerDirection") == -1)
            gameObject.GetComponent<SpriteRenderer>().sprite = leftDeathSprite;
        else
            gameObject.GetComponent<SpriteRenderer>().sprite = rightDeathSprite;


        Destroy(this);
    }

    private void OnTriggerEnter2D(Collider2D collision) {
        if (collision.gameObject.tag == "Hitbox") {
            DealHitMelee hitbox = collision.GetComponent<DealHitMelee>();
            TakeHit(hitbox.getDamage());
        }
    }

    void playAttack() {
        gridScript.playRandomPattern();
        if (isSecondStage )
            summonGhouls();
        animator.SetBool("isAttacking", false);
        isAttacking = false;

    }

    void summonGhouls() {        
        foreach(GameObject ghoul in bossGhouls) {
            Enemy ghoulScript = ghoul.GetComponent<Enemy>();
            if (!ghoul.activeSelf) {
                ghoul.SetActive(true);
                transform.parent.parent.GetComponent<Room>().enemies.Add(ghoul);
            }
            else if (ghoulScript.isDead)
                ghoul.GetComponent<Enemy>().startRevive();

        }
    }
}
