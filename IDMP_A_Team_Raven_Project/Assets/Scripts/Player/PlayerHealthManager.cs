using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;

public class PlayerHealthManager : MonoBehaviour
{
    public FloatValue playerHealth;
    public Slider healthSlider;
    public GameObject player;
    public GameObject checkpoint;
    public SpriteRenderer blackScreen;

    public bool killPlayer = false;
    private bool dying;

    //TODO - call in whatever script handles player being hit in combat

    

    // Start is called before the first frame update
    void Start()
    {
        onStart();
    }

    private void Update()
    {
        if (killPlayer)
        {
            playerDeath();
            killPlayer = false;
        }
    }

    private void onStart()
    {
        dying = false;
        healthSlider.maxValue = playerHealth.initialValue;
        healthSlider.value = playerHealth.initialValue;
    }

    public void AddHealth()
    {
        healthSlider.value = playerHealth.runTimeValue;
        if (healthSlider.value > healthSlider.maxValue)
        {
            healthSlider.value = healthSlider.maxValue;
            playerHealth.runTimeValue = playerHealth.initialValue;
        }
    }

    public void DecreaseHealth()
    {
        //Debug.Log("should be taking dmg in health manager");
        healthSlider.value = playerHealth.runTimeValue;
        //Debug.Log("health = " + playerHealth.runTimeValue);
        if (healthSlider.value <= 0)
        {
            healthSlider.value = 0;
            playerHealth.runTimeValue = 0;
            if (!dying)
            {
                dying = true;
                playerDeath();
            }
        }
    }

    private void playerDeath()
    {
        StartCoroutine(deathSequence());
    }

    IEnumerator deathSequence()
    {
        // get relevant objects and scripts
        PlayerMovement moveScript = player.GetComponent<PlayerMovement>();
        Rigidbody2D rb2d = player.GetComponent<Rigidbody2D>();
        // freeze player's rigidbody
        rb2d.constraints = RigidbodyConstraints2D.FreezeAll;
        // TODO - freeze other actions
        // trigger death anim
        moveScript.onDeath();

        // allow death anim to play
        yield return new WaitForSeconds(1f);

        // fade to black - blackscreen is attached to camera
        float elapsed = 0f;
        float max = 2f;
        while (elapsed < max)
        {
            blackScreen.color = new Color(0f, 0f, 0f, elapsed / max);
            elapsed += Time.deltaTime;
            yield return null;
        }
        blackScreen.color = Color.black;

        // stay on black screen for a second
        yield return new WaitForSeconds(1f);

        // revive player and reset position/state at checkpoint
        moveScript.onRevive();
        checkpoint.GetComponent<Checkpoint>().CheckPointHandlePlayerDeath();
        // unfreeze player's rigidbody
        rb2d.constraints = RigidbodyConstraints2D.FreezeRotation;
        onStart();

        // fade from black
        elapsed = 0f;
        while (elapsed < max)
        {
            blackScreen.color = new Color(0f, 0f, 0f, 1f - (elapsed / max));
            elapsed += Time.deltaTime;
            yield return null;
        }
        blackScreen.color = Color.clear;

    }
}
