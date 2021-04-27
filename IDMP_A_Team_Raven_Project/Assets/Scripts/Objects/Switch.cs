using UnityEngine;

public class Switch : MonoBehaviour
{
    public bool active;
    public BoolValue storedValue;
    public Sprite activeSprite;
    private SpriteRenderer switchSprite;
    public Door thisDoor;

    // Start is called before the first frame update
    void Start()
    {
        active = storedValue.runTimeValue;
        switchSprite = GetComponent<SpriteRenderer>();
    }


    public void ActivateSwitch()
    {
        active = true;
        storedValue.runTimeValue = active;
        thisDoor.Open();
        switchSprite.sprite = activeSprite;
    }

    public void OnTriggerEnter2D(Collider2D other)
    {
        // Is it the player?
        if (other.CompareTag("Player"))
        {
            ActivateSwitch();
        }
    }
}
