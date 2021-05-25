using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class AfterImageScript : MonoBehaviour
{
    protected float afterImageDelay = 0.3f;         //The delay between creating new after images
    [SerializeField] protected BoxCollider2D hitbox;
    private Coroutine coroutine;

    //creates an after image trail from origin to newPos
    public void createAfterImageTrail(Vector2 original, Vector2 newPos, Sprite sprite) {
        Vector2 direction = (Vector2)(newPos - original);
        coroutine = StartCoroutine(createTrail(original, newPos, sprite, direction));
        

    }

    //creates an after image at origin
    public void createAfterImage(Transform original, Sprite sprite) {

        //Create new after image game object with a sprite renderer and timer
        GameObject afterImage = new GameObject("After Image");
        afterImage.transform.position = original.transform.position;
        SpriteRenderer afterImageSR = afterImage.AddComponent<SpriteRenderer>();
        AfterImageFadeScript timerScript = afterImage.AddComponent<AfterImageFadeScript>();
        afterImageSR.sprite = sprite;


        //Change the after image sprite to have a blue hue
        afterImageSR.color = new Color(0f, 0.25f, 1f, 1f);

        afterImageSR.sortingLayerName = "Player";

        //Instantiate the after image and start fading it
        timerScript.startFade(afterImageSR, afterImageDelay);
    }

    IEnumerator createTrail(Vector2 original, Vector2 newPos, Sprite sprite, Vector2 direction) {
        //How long it takes for the after images to get to the newPos
        float catchUpDuration = 0.5f;
        float timer = 0f;

        float imageDelay = 0.1f;
        float imageDelayTimer = 0f;
        GetComponent<SpriteRenderer>().enabled = false;
        hitbox.enabled = false;
        while(timer < catchUpDuration) {
            if(imageDelayTimer < imageDelay) {
                imageDelayTimer += Time.deltaTime;
            }
            else {
                GameObject afterImage = new GameObject("After Image");
                afterImage.transform.position =
                    Vector2.Lerp(original, newPos, timer / catchUpDuration);
                SpriteRenderer afterImageSR = afterImage.AddComponent<SpriteRenderer>();
                AfterImageFadeScript timerScript = afterImage.AddComponent<AfterImageFadeScript>();
                afterImageSR.sprite = sprite;

                //Change the after image sprite to have a blue hue
                afterImageSR.color = new Color(0.25f, 0.25f, 0.25f, 1f);

                afterImageSR.sortingLayerName = "Player";


                timerScript.startFade(afterImageSR, afterImageDelay);
                imageDelayTimer = 0f;
            }

            timer += Time.deltaTime;
            yield return null;
        }
        
        GetComponent<SpriteRenderer>().enabled = true;
        hitbox.enabled = true;
    }
}
